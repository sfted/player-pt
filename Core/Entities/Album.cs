﻿using Player.Core.Entities.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Media.Imaging;

namespace Player.Core.Entities
{
    public class Album : BaseEntity, IPlayable
    {
        public string Title { get; set; }
        public uint Year { get; set; }
        public uint TrackCount { get; set; }
        public uint DiscCount { get; set; }
        //public string AlbumArtUri { get; set; }

        public int? AlbumArtId { get; set; }
        public virtual AlbumArt Art { get; set; }

        public List<Track> Tracks { get; set; } = new List<Track>();
        public List<Artist> Artists { get; set; } = new List<Artist>();
        public List<Genre> Genres { get; set; } = new List<Genre>();


        private BitmapSource artSmall;
        private BitmapSource artMedium;
        private BitmapSource artBig;

        [NotMapped]
        public BitmapSource ArtSmall
        {
            get
            {
                if (artSmall == null)
                    artSmall = LoadAlbumArt(AlbumArtId, 150);

                return artSmall;
            }
            set
            {
                artSmall = value;
                NotifyPropertyChanged(nameof(ArtSmall));
            }
        }

        [NotMapped]
        public BitmapSource ArtMedium
        {
            get
            {
                if (artMedium == null)
                    artMedium = LoadAlbumArt(AlbumArtId, 300);

                return artMedium;
            }
            set
            {
                artMedium = value;
                NotifyPropertyChanged(nameof(ArtMedium));
            }
        }

        [NotMapped]
        public BitmapSource ArtBig
        {
            get
            {
                if (artBig == null)
                    artBig = LoadAlbumArt(AlbumArtId, 450);

                return artBig;
            }
            set
            {
                artBig = value;
                NotifyPropertyChanged(nameof(ArtBig));
            }
        }

        private static BitmapImage LoadAlbumArt(int? id, int size)
        {
            if (id != null)
            {
                using var db = new ApplicationContext();

                var data = db.GetAlbumArtDataById((int)id);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(data);
                bitmap.CreateOptions = BitmapCreateOptions.DelayCreation;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.DecodePixelWidth = size;
                bitmap.DecodePixelHeight = size;
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
            else
                return null;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
