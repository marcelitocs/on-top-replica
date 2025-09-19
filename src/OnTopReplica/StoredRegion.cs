using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace OnTopReplica {

    /// <summary>
    /// Represents a stored region.
    /// </summary>
	public class StoredRegion {

        /// <summary>
        /// Creates a new instance of the stored region.
        /// </summary>
        /// <param name="r">The region to store.</param>
        /// <param name="name">The name of the region.</param>
        public StoredRegion(ThumbnailRegion r, string name) {
            Region = r;
            Name = name;
        }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public ThumbnailRegion Region {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the region.
        /// </summary>
		public string Name {
			get;
			set;
		}

		public override string ToString() {
			return Name;
		}

	}

}
