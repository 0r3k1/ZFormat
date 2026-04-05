using ZFormat.Core;

namespace ZFormat.Layouts {
    public class ZTabItem {
        public string Title { get; set; }
        public string Icon { get; set; }
        public ZControl Content { get; set; } // El panel o control que se mostrará

        public ZTabItem(string title, string icon, ZControl content) {
            Title = title;
            Icon = icon;
            Content = content;
        }
    }
}