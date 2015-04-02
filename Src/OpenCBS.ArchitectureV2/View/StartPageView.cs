﻿using System.Drawing;
using System.Windows.Forms;
using OpenCBS.ArchitectureV2.Interface.Presenter;
using OpenCBS.ArchitectureV2.Interface.View;

namespace OpenCBS.ArchitectureV2.View
{
    public partial class StartPageView : Form, IStartPageView
    {
        public StartPageView()
        {
            InitializeComponent();
            SizeChanged += (sender, e) => OnResize();
            new ToolTip().SetToolTip(_englishPictureBox, "English");
            new ToolTip().SetToolTip(_frenchPictureBox, "Français");
            new ToolTip().SetToolTip(_russianPictureBox, "Русский");
            new ToolTip().SetToolTip(_spanishPictureBox, "Español");
            new ToolTip().SetToolTip(_portuguesePictureBox, "Português");
        }

        public void Attach(IStartPagePresenterCallbacks presenterCallbacks)
        {
            _newPersonItem.Click += (sender, e) => presenterCallbacks.AddPerson();
            _newGroupItem.Click += (sender, e) => presenterCallbacks.AddGroup();
            _newVillageBankItem.Click += (sender, e) => presenterCallbacks.AddVillageBank();
            _newCompanyItem.Click += (sender, e) => presenterCallbacks.AddCompany();
            _searchClientButton.Click += (sender, e) => presenterCallbacks.SearchClient();
            _searchLoanButton.Click += (sender, e) => presenterCallbacks.SearchLoan();

            _englishPictureBox.Click += (sender, e) => presenterCallbacks.ChangeLanguage("en-US");
            _frenchPictureBox.Click += (sender, e) => presenterCallbacks.ChangeLanguage("fr");
            _russianPictureBox.Click += (sender, e) => presenterCallbacks.ChangeLanguage("ru-RU");
            _spanishPictureBox.Click += (sender, e) => presenterCallbacks.ChangeLanguage("es-ES");
            _portuguesePictureBox.Click += (sender, e) => presenterCallbacks.ChangeLanguage("pt");

            _siteLink.Click += (sender, e) => presenterCallbacks.OpenUrl("http://www.opencbs.com");
            _userGuideLink.Click +=(sender, e) => presenterCallbacks.OpenUrl("http://opencbs.com/uploads/User%20guide%20OpenCBS%2014.11.pdf");
            _forumLink.Click += (sender, e) => presenterCallbacks.OpenUrl("http://opencbs.freeforums.net/");
            _contactLink.Click += (sender, e) => presenterCallbacks.OpenEmail("contact@opencbs.com", "I have a question about OPENCBS");
        }

        private void OnResize()
        {
            SuspendLayout();

            var centerX = Size.Width / 2;
            var centerY = Size.Height / 2;

            // Logo
            var x = centerX - _logoPictureBox.Width / 2;
            var y = centerY - _logoPictureBox.Height - 150;
            _logoPictureBox.Location = new Point(x, y);

            // Buttons
            x = centerX - _buttonPanel.Width / 2;
            y = centerY - _buttonPanel.Height / 2;
            _buttonPanel.Location = new Point(x, y);

            // Links
            x = centerX - _linkPanel.Width / 2;
            y = Size.Height - _linkPanel.Height - 100;
            _linkPanel.Location = new Point(x, y);

            // Languages (Flags)
            x = centerX - _languagePanel.Width / 2;
            y = Size.Height - _languagePanel.Height - 50;
            _languagePanel.Location = new Point(x, y);

            ResumeLayout();
        }
    }
}
