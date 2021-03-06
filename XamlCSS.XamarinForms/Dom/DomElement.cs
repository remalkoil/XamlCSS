﻿using System.Collections.Generic;
using AngleSharp.Dom;
using XamlCSS.Dom;
using Xamarin.Forms;
using System;
using XamlCSS.Windows.Media;
using System.Linq;

namespace XamlCSS.XamarinForms.Dom
{
    public abstract class DomElement : DomElementBase<BindableObject, BindableProperty>, IDisposable
    {
        public DomElement(BindableObject dependencyObject, ITreeNodeProvider<BindableObject> treeNodeProvider)
            : base(dependencyObject, treeNodeProvider)
        {
            RegisterChildrenChangeHandler();
        }

        private void RegisterChildrenChangeHandler()
        {
            VisualTreeHelper.SubTreeAdded += ElementAdded;
            VisualTreeHelper.SubTreeRemoved += ElementRemoved;
        }

        public new void Dispose()
        {
            UnregisterChildrenChangeHandler();

            base.Dispose();
        }

        private void UnregisterChildrenChangeHandler()
        {
            VisualTreeHelper.SubTreeAdded -= ElementAdded;
            VisualTreeHelper.SubTreeRemoved -= ElementRemoved;
        }

        private void ElementAdded(object sender, EventArgs e)
        {
            var parentElement = (sender as Element)?.Parent;

            if (parentElement == dependencyObject)
            {
                this.ResetChildren();
            }
        }

        private void ElementRemoved(object sender, EventArgs e)
        {
            if (Children.Any(x => ((DomElement)x).Element == sender))
            {
                this.ResetChildren();
            }
        }

        protected override IHtmlCollection<IElement> CreateCollection(IEnumerable<IElement> list)
        {
            return new ElementCollection(list, treeNodeProvider);
        }
        protected override INamedNodeMap CreateNamedNodeMap(BindableObject dependencyObject)
        {
            return new NamedNodeMap(dependencyObject);
        }

        protected override IHtmlCollection<IElement> GetChildElements(BindableObject dependencyObject)
        {
            return new ElementCollection(this, treeNodeProvider);
        }
        protected override INodeList GetChildNodes(BindableObject dependencyObject)
        {
            return new NamedNodeList(this, treeNodeProvider);
        }
        protected override INodeList CreateNodeList(IEnumerable<INode> nodes)
        {
            return new NamedNodeList(nodes, treeNodeProvider);
        }
        protected override ITokenList GetClassList(BindableObject dependencyObject)
        {
            var list = new TokenList();

            var classNames = Css.GetClass(dependencyObject)?.Split(classSplitter, StringSplitOptions.RemoveEmptyEntries);
            if (classNames?.Length > 0)
            {
                list.AddRange(classNames);
            }

            return list;
        }
        protected override string GetId(BindableObject dependencyObject)
        {
            return Css.GetId(dependencyObject);
        }
    }
}
