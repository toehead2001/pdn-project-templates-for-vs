using PaintDotNet;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        // Names of the properties
        private enum PropertyNames
        {
            Creator
        }

        // Defaults
        private const string defaultCreator = "";

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
        /// Add properties to the dialog
        /// </summary>
        public override PropertyCollection OnCreateSavePropertyCollection()
        {
            Property[] props = new Property[]
            {
                new StringProperty(PropertyNames.Creator, defaultCreator, 256)
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
            configUI.SetPropertyControlValue(PropertyNames.Creator, ControlInfoPropertyNames.DisplayName, "Name of creator");

            return configUI;
        }

        /// <summary>
        /// Saves a document to a stream respecting the properties
        /// </summary>
        protected override void OnSaveT(Document input, Stream output, PropertyBasedSaveConfigToken token, Surface scratchSurface, ProgressEventHandler progressCallback)
        {
            // add some code here ...
        }

        /// <summary>
        /// Creates a document from a stream
        /// </summary>
        protected override Document OnLoad(Stream input)
        {
            // add some code here ...

            return null;
        }
    }
}
