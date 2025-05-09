using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Layouter.Models
{
    public class DesktopIcon : ObservableObject
    {
        private string id;
        private string name;
        private string iconPath;
        private Point position;
        private Size size = new Size(48, 48);
        private bool isDragging;
        private IconType iconType = IconType.Shortcut;
        private double textSize = 12d;

        public string Id 
        { 
            get => id; 
            set => SetProperty(ref id, value); 
        }

        public string Name 
        { 
            get => name; 
            set => SetProperty(ref name, value); 
        }

        public string IconPath 
        { 
            get => iconPath; 
            set => SetProperty(ref iconPath, value); 
        }

        public Point Position 
        { 
            get => position; 
            set => SetProperty(ref position, value); 
        }

        public Size Size 
        { 
            get => size; 
            set => SetProperty(ref size, value); 
        }
        
        public bool IsDragging
        {
            get => isDragging;
            set => SetProperty(ref isDragging, value);
        }

        public IconType IconType
        {
            get => iconType;
            set => SetProperty(ref iconType, value);
        }

        public double TextSize
        {
            get => textSize;
            set => SetProperty(ref textSize, value);
        }

        public DesktopIcon()
        {
            // 生成唯一ID
            Id = Guid.NewGuid().ToString();
            Size = new Size(48, 48); // 默认大小
        }

        public DesktopIcon Clone()
        {
            return new DesktopIcon
            {
                Name = this.Name,
                IconPath = this.IconPath,
                Position = this.Position,
                Size = this.Size,
                IconType = this.IconType,
                TextSize = this.TextSize
            };
        }
    }

    public enum IconType
    {
        Shortcut = 0,
        Shell,
        File,
        Folder
    }
}
