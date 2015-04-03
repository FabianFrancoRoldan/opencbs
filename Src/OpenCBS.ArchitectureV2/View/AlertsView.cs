﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;
using OpenCBS.ArchitectureV2.Command;
using OpenCBS.ArchitectureV2.Interface.Presenter;
using OpenCBS.ArchitectureV2.Model;

namespace OpenCBS.ArchitectureV2.View
{
    public partial class AlertsView : Form, IAlertsView
    {
        public AlertsView()
        {
            InitializeComponent();
            ShowLateLoans = true;
            ShowPerformingLoans = true;
            _lateLoansItem.Checked = true;
            _performingLoansItem.Checked = true;
            _clearSearchButton.Visible = false;
            SetUp();
        }

        public void Attach(IAlertsPresenterCallbacks presenterCallbacks)
        {
            FormClosing += (sender, e) => presenterCallbacks.DetachView();
            _reloadButton.Click += (sender, e) => presenterCallbacks.Reload();

            _performingLoansItem.Click += (sender, e) =>
            {
                ShowPerformingLoans = !ShowPerformingLoans;
                _performingLoansItem.Checked = ShowPerformingLoans;
                presenterCallbacks.Refresh();
            };

            _lateLoansItem.Click += (sender, e) =>
            {
                ShowLateLoans = !ShowLateLoans;
                _lateLoansItem.Checked = ShowLateLoans;
                presenterCallbacks.Refresh();
            };

            _pendingLoansItem.Click += (sender, e) =>
            {
                ShowPendingLoans = !ShowPendingLoans;
                _pendingLoansItem.Checked = ShowPendingLoans;
                presenterCallbacks.Refresh();
            };

            _validatedLoansItem.Click += (sender, e) =>
            {
                ShowValidatedLoans = !ShowValidatedLoans;
                _validatedLoansItem.Checked = ShowValidatedLoans;
                presenterCallbacks.Refresh();
            };

            _postponedLoansItem.Click += (sender, e) =>
            {
                ShowPostponedLoans = !ShowPostponedLoans;
                _postponedLoansItem.Checked = ShowPostponedLoans;
                presenterCallbacks.Refresh();
            };

            _pendingSavingsItem.Click += (sender, e) =>
            {
                ShowPendingSavings = !ShowPendingSavings;
                _pendingSavingsItem.Checked = ShowPendingSavings;
                presenterCallbacks.Refresh();
            };

            _overdraftSavingsItem.Click += (sender, e) =>
            {
                ShowOverdraftSavings = !ShowOverdraftSavings;
                _overdraftSavingsItem.Checked = ShowOverdraftSavings;
                presenterCallbacks.Refresh();
            };
        }

        private void UpdateTitle()
        {
            Text = string.Format("Alerts ({0})", _alertsListView.Items.Count);
        }

        public void SetAlerts(List<Alert> alerts)
        {
            _alertsListView.SetObjects(alerts);
            _alertsListView.Sort(_lateDaysColumn, SortOrder.Descending);
            UpdateTitle();
        }

        private static void OnFormatAlertRow(object sender, FormatRowEventArgs e)
        {
            var alert = (Alert) e.Model;
            e.Item.BackColor = alert.BackColor;
            e.Item.ForeColor = alert.ForeColor;
        }

        private void SetUp()
        {
            _dateColumn.AspectToStringConverter = delegate(object value)
            {
                var date = (DateTime) value;
                return date.ToShortDateString();
            };
            _amountColumn.AspectToStringConverter = delegate(object value)
            {
                var amount = (decimal) value;
                return amount.ToString("N2");
            };
            _alertsListView.FormatRow += OnFormatAlertRow;

            _searchTextBox.TextChanged += (sender, e) => OnSearchTextChanged();
            (_searchTextBox.Control as TextBox).SetHint("Search");
            _clearSearchButton.Click += (sender, e) => _searchTextBox.Text = string.Empty;
        }

        public void StartProgress()
        {
            Cursor = Cursors.WaitCursor;
            _toolStrip.Enabled = false;
            Text = "Alerts (loading...)";
        }

        public void StopProgress()
        {
            Cursor = Cursors.Default;
            _toolStrip.Enabled = true;
        }

        public bool ShowPerformingLoans { get; private set; }

        public bool ShowLateLoans { get; private set; }

        public bool ShowPendingLoans { get; private set; }

        public bool ShowValidatedLoans { get; private set; }

        public bool ShowPostponedLoans { get; private set; }

        public bool ShowPendingSavings { get; private set; }

        public bool ShowOverdraftSavings { get; private set; }

        private void OnSearchTextChanged()
        {
            var filter = _searchTextBox.Text.ToLower();
            if (string.IsNullOrEmpty(filter))
            {
                _alertsListView.UseFiltering = false;
                _clearSearchButton.Visible = false;
                UpdateTitle();
                return;
            }

            _alertsListView.UseFiltering = true;
            _alertsListView.ModelFilter = new ModelFilter(delegate(object x)
            {
                var alert = (Alert) x;
                return alert.ContractCode.ToLower().Contains(filter)
                       || alert.ClientName.ToLower().Contains(filter)
                       || alert.LoanOfficer.ToLower().Contains(filter);
            });
            _clearSearchButton.Visible = true;
            UpdateTitle();
        }
    }
}
