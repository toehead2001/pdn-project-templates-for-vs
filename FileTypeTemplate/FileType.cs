using PaintDotNet;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace $safeprojectname$
{
    public sealed class $safeprojectname$Factory : IFileTypeFactory
    {
        public FileType[] GetFileTypeInstances()
        {
            return new[] { new $safeprojectname$Plugin() };
        }
    }

    internal class $safeprojectname$Plugin : PropertyBasedFileType
    {
        private const string HeaderSignature = ".PDN";

        // Names of the properties
        private enum PropertyNames
        {
            Invert
        }

        // Defaults
        private const bool defaultInvert = false;

        /// <summary>
        /// Constructs a ExamplePropertyBasedFileType instance
        /// </summary>
        internal $safeprojectname$Plugin()
            : base(
                "MyFileType",
                new FileTypeOptions
                {
                    LoadExtensions = new string[] { ".foo", ".bar" },
                    SaveExtensions = new string[] { ".foo", ".bar" },
                    SupportsCancellation = true,
                    SupportsLayers = false
                })
        {
        }

        /// <summary>
        /// Determines if the document was saved without altering the pixel values.
        ///
        /// Any settings that change the pixel values should return 'false'.
        ///
        /// Because Paint.NET prompts the user to flatten the image, flattening should not be
        /// considered.
        /// For example, a 32-bit PNG will return 'true' even if the document has multiple layers.
        /// </summary>
        protected override bool IsReflexive(PropertyBasedSaveConfigToken token)
        {
            bool invert = token.GetProperty<BooleanProperty>(PropertyNames.Invert).Value;

            return invert == false;
        }

        /// <summary>
        /// Add properties to the dialog
        /// </summary>
        public override PropertyCollection OnCreateSavePropertyCollection()
        {
            Property[] props = new Property[]
            {
                new BooleanProperty(PropertyNames.Invert, defaultInvert, false)
            };

            PropertyCollectionRule[] propRules = new PropertyCollectionRule[]
            {

            };

            return new PropertyCollection(props, propRules);
        }

        /// <summary>
        /// Adapt properties in the dialog (DisplayName, ...)
        /// </summary>
        public override ControlInfo OnCreateSaveConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultSaveConfigUI(props);
            configUI.SetPropertyControlValue(PropertyNames.Invert, ControlInfoPropertyNames.DisplayName, string.Empty);
            configUI.SetPropertyControlValue(PropertyNames.Invert, ControlInfoPropertyNames.Description, "Invert");

            return configUI;
        }

        /// <summary>
        /// Saves a document to a stream respecting the properties
        /// </summary>
        protected override void OnSaveT(Document input, Stream output, PropertyBasedSaveConfigToken token, Surface scratchSurface, ProgressEventHandler progressCallback)
        {
            bool invert = token.GetProperty<BooleanProperty>(PropertyNames.Invert).Value;

            using (RenderArgs args = new RenderArgs(scratchSurface))
            {
                // Render a flattened view of the Document to the scratch surface.
                input.Render(args, true);
            }

            if (invert)
            {
                new UnaryPixelOps.Invert().Apply(scratchSurface, scratchSurface.Bounds);
            }

            // The stream paint.net hands us must not be closed.
            using (BinaryWriter writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen: true))
            {
                // Write the file header.
                writer.Write(Encoding.ASCII.GetBytes(HeaderSignature));
                writer.Write(scratchSurface.Width);
                writer.Write(scratchSurface.Height);

                for (int y = 0; y < scratchSurface.Height; y++)
                {
                    // Report progress if the callback is not null.
                    if (progressCallback != null)
                    {
                        double percent = (double)y / scratchSurface.Height;

                        progressCallback(null, new ProgressEventArgs(percent));
                    }

                    for (int x = 0; x < scratchSurface.Width; x++)
                    {
                        // Write the pixel values.
                        ColorBgra color = scratchSurface[x, y];

                        writer.Write(color.Bgra);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a document from a stream
        /// </summary>
        protected override Document OnLoad(Stream input)
        {
            Document doc = null;

            // The stream paint.net hands us must not be closed.
            using (BinaryReader reader = new BinaryReader(input, Encoding.UTF8, leaveOpen: true))
            {
                // Read and validate the file header.
                byte[] headerSignature = reader.ReadBytes(4);

                if (Encoding.ASCII.GetString(headerSignature) != HeaderSignature)
                {
                    throw new FormatException("Invalid file signature.");
                }

                int width = reader.ReadInt32();
                int height = reader.ReadInt32();

                // Create a new Document.
                doc = new Document(width, height);

                // Create a background layer.
                BitmapLayer layer = Layer.CreateBackgroundLayer(width, height);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // Read the pixel values from the file.
                        uint bgraColor = reader.ReadUInt32();

                        layer.Surface[x, y] = ColorBgra.FromUInt32(bgraColor);
                    }
                }

                // Add the new layer to the Document.
                doc.Layers.Add(layer);
            }

            return doc;
        }
    }
}
