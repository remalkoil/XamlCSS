﻿using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using XamlCSS.ComponentModel;
using XamlCSS.Dom;

namespace XamlCSS.XamarinForms
{
    public class DependencyPropertyService : IDependencyPropertyService<BindableObject, BindableObject, Style, BindableProperty>
    {
        private ITypeConverterProvider<TypeConverter> typeConverterProvider;

        public DependencyPropertyService()
        {
            this.typeConverterProvider = new XamarinTypeConverterProvider();
        }

        public BindableProperty GetBindableProperty(BindableObject frameworkElement, string propertyName)
        {
            return GetBindableProperty(frameworkElement.GetType(), propertyName);
        }
        public BindableProperty GetBindableProperty(Type bindableObjectType, string propertyName)
        {
            string dpName = $"{propertyName}Property";
            var dpFields = TypeHelpers.DeclaredFields(bindableObjectType);
            var dpField = dpFields.FirstOrDefault(i => i.Name == dpName);

            if (dpField != null)
                return dpField.GetValue(null) as BindableProperty;
            return null;
        }

        public object GetBindablePropertyValue(BindableObject frameworkElement, BindableProperty bindableProperty, object propertyValue)
        {
            return GetBindablePropertyValue(frameworkElement.GetType(), bindableProperty, propertyValue);
        }

        public object GetClrValue(Type propertyType, string propertyValueString)
        {
            if (!(propertyType.GetTypeInfo()
                .IsAssignableFrom(propertyValueString.GetType().GetTypeInfo())))
            {
                TypeConverter converter = null;

                converter = typeConverterProvider.GetConverterFromProperty(propertyValueString, propertyType);

                if (converter == null)
                    converter = typeConverterProvider.GetConverter(propertyType);
                if (converter != null)
                    return converter.ConvertFromInvariantString(propertyValueString);

                else if (propertyType == typeof(bool))
                    return propertyValueString.Equals("true");
                else if (propertyType == typeof(Color))
                    return Color.FromHex(propertyValueString as string);
                else if (propertyType == typeof(LayoutOptions))
                    return propertyType.GetRuntimeFields().First(x => x.Name == propertyValueString as string).GetValue(null);
                else if (propertyType.GetTypeInfo().IsEnum)
                    return Enum.Parse(propertyType, propertyValueString as string);
                else
                    return Convert.ChangeType(propertyValueString, propertyType);
            }

            return propertyValueString;
        }

        public object GetBindablePropertyValue(Type frameworkElementType, BindableProperty bindableProperty, object propertyValue)
        {
            if (!(bindableProperty.ReturnType.GetTypeInfo()
                .IsAssignableFrom(propertyValue.GetType().GetTypeInfo())))
            {
                Type propertyType = bindableProperty.ReturnType;
                TypeConverter converter = null;

                converter = typeConverterProvider.GetConverterFromProperty(bindableProperty.PropertyName, frameworkElementType);

                if (converter == null)
                    converter = typeConverterProvider.GetConverter(propertyType);
                if (converter != null)
                    propertyValue = converter.ConvertFromInvariantString(propertyValue as string);

                else if (propertyType == typeof(bool))
                    propertyValue = propertyValue.Equals("true");
                else if (propertyType == typeof(Color))
                    propertyValue = Color.FromHex(propertyValue as string);
                else if (propertyType == typeof(LayoutOptions))
                    propertyValue = propertyType.GetRuntimeFields().First(x => x.Name == propertyValue as string).GetValue(null);
                else if (propertyType.GetTypeInfo().IsEnum)
                    propertyValue = Enum.Parse(propertyType, propertyValue as string);
                else
                    propertyValue = Convert.ChangeType(propertyValue, propertyType);
            }

            return propertyValue;
        }

        public string[] GetAppliedMatchingStyles(BindableObject obj)
        {
            return Css.GetAppliedMatchingStyles(obj);
        }

        public string GetClass(BindableObject obj)
        {
            return Css.GetClass(obj);
        }

        public bool? GetHadStyle(BindableObject obj)
        {
            return Css.GetHadStyle(obj);
        }

        public Style GetInitialStyle(BindableObject obj)
        {
            return Css.GetInitialStyle(obj);
        }

        public string[] GetMatchingStyles(BindableObject obj)
        {
            return Css.GetMatchingStyles(obj);
        }

        public string GetName(BindableObject obj)
        {
            return Css.GetId(obj);
        }

        public StyleDeclarationBlock GetStyle(BindableObject obj)
        {
            return Css.GetStyle(obj);
        }

        public StyleSheet GetStyleSheet(BindableObject obj)
        {
            return Css.GetStyleSheet(obj);
        }

        public bool GetHandledCss(BindableObject obj)
        {
            return Css.GetHandledCss(obj);
        }

        public void SetAppliedMatchingStyles(BindableObject obj, string[] value)
        {
            Css.SetAppliedMatchingStyles(obj, value);
        }

        public void SetClass(BindableObject obj, string value)
        {
            Css.SetClass(obj, value);
        }

        public void SetHadStyle(BindableObject obj, bool? value)
        {
            Css.SetHadStyle(obj, value);
        }

        public void SetInitialStyle(BindableObject obj, Style value)
        {
            Css.SetInitialStyle(obj, value);
        }

        public void SetMatchingStyles(BindableObject obj, string[] value)
        {
            Css.SetMatchingStyles(obj, value);
        }

        public void SetName(BindableObject obj, string value)
        {
            Css.SetId(obj, value);
        }

        public void SetStyle(BindableObject obj, StyleDeclarationBlock value)
        {
            Css.SetStyle(obj, value);
        }

        public void SetStyleSheet(BindableObject obj, StyleSheet value)
        {
            Css.SetStyleSheet(obj, value);
        }

        public void SetHandledCss(BindableObject obj, bool value)
        {
            Css.SetHandledCss(obj, value);
        }

        public bool IsLoaded(BindableObject obj)
        {
            return (obj as Element)?.Parent != null;
        }

        public void RegisterLoadedOnce(BindableObject frameworkElement, Action<object> func)
        {
            EventHandler handler = null;
            handler = (s, e) =>
            {
                frameworkElement.BindingContextChanged -= handler;
                func(s);
            };
            frameworkElement.BindingContextChanged += handler;
        }

        public IDomElement<BindableObject> GetDomElement(BindableObject obj)
        {
            return Css.GetDomElement(obj) as IDomElement<BindableObject>;
        }

        public void SetDomElement(BindableObject obj, IDomElement<BindableObject> value)
        {
            Css.SetDomElement(obj, value);
        }
    }
}
