﻿using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using BolapanControl.ItemsFilter.Model;

namespace BolapanControl.ItemsFilter.View {
    /// <summary>
    /// Defile View control for IStringFilter model.
    /// </summary>
    [ModelView]
    [TemplatePart(Name = StringFilterView.PART_FilterType, Type = typeof(Selector))]
    public class StringFilterView : FilterViewBase<IStringFilter>, IFilterView {
        internal const string PART_FilterType = "PART_FilterType";
        static StringFilterView() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StringFilterView),
                new FrameworkPropertyMetadata(typeof(StringFilterView)));

        }
        /// <summary>
        /// Instance of a selector allowing to choose the filtering mode
        /// </summary>
        private Selector _selectorFilterType;

        private string _mode;

        /// <summary>
        /// Create new instance of StringFilterView.
        /// </summary>
        public StringFilterView() : base() { }
        /// <summary>
        /// Create new instance of StringFilterView and accept IStringFilter model.
        /// </summary>
        /// <param name="model"></param>
        public StringFilterView(object model){
            base.Model = model as IStringFilter;
        }
   
        /// <summary>
        /// Called when the control template is applied to this control
        /// </summary>
        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
            _selectorFilterType = GetTemplateChild(PART_FilterType) as Selector;
            if (_selectorFilterType != null) {
                _selectorFilterType.ItemsSource = GetFilterModes();
            }
        }

        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                Model.Mode = GetMode(value);
            }
        }

        private StringFilterMode GetMode(string value)
        {
            switch (value)
            {
                case "Започни с":
                    return StringFilterMode.StartsWith;
                case "Завършва на":
                    return StringFilterMode.EndsWith;
                case "Равна на":
                    return StringFilterMode.Equals;
            }
            return StringFilterMode.Contains;
        }

        private IEnumerable<string> GetFilterModes() {
            //foreach (var item in typeof(StringFilterMode).GetEnumValues()) {
            //    yield return (StringFilterMode)item;
            //}
            return new List<string> {"Започни с","Завършва на","Съдържа","Равна на"};
        }
    }
}
