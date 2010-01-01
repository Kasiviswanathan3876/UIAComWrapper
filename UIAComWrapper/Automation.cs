﻿// (c) Copyright Michael Bernstein, 2009.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System;
using UIAComWrapperInternal;

namespace System.Windows.Automation
{
    class Automation
    {
        
        private static readonly UIAutomationClient.IUIAutomation factory = new UIAutomationClient.CUIAutomationClass();
        public static readonly Condition ContentViewCondition = Condition.Wrap(Factory.ContentViewCondition);
        public static readonly Condition ControlViewCondition = Condition.Wrap(Factory.ControlViewCondition);
        public static readonly Condition RawViewCondition = Condition.Wrap(Factory.RawViewCondition);        

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Automation()
        {
        }

        Automation()
        {
        }

        internal static UIAutomationClient.IUIAutomation Factory
        {
            get
            {
                return factory;
            }
        }

        public static void AddAutomationEventHandler(AutomationEvent eventId, AutomationElement element, TreeScope scope, AutomationEventHandler eventHandler)
        {
            Utility.ValidateArgumentNonNull(element, "element");
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
            Utility.ValidateArgument(eventId != AutomationElement.AutomationFocusChangedEvent, "Use FocusChange notification instead");
            Utility.ValidateArgument(eventId != AutomationElement.StructureChangedEvent, "Use StructureChange notification instead");
            Utility.ValidateArgument(eventId != AutomationElement.AutomationPropertyChangedEvent, "Use PropertyChange notification instead");

            try
            {
                BasicEventListener listener = new BasicEventListener(eventId, element, eventHandler);
                Factory.AddAutomationEventHandler(
                    eventId.Id,
                    element.NativeElement,
                    (UIAutomationClient.TreeScope)scope,
                    CacheRequest.CurrentNativeCacheRequest,
                    listener);
                ClientEventList.Add(listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static void AddAutomationFocusChangedEventHandler(AutomationFocusChangedEventHandler eventHandler)
        {
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

            try
            {
                FocusEventListener listener = new FocusEventListener(eventHandler);
                Factory.AddFocusChangedEventHandler(CacheRequest.CurrentNativeCacheRequest, listener);
                ClientEventList.Add(listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static void AddAutomationPropertyChangedEventHandler(AutomationElement element, TreeScope scope, AutomationPropertyChangedEventHandler eventHandler, params AutomationProperty[] properties)
        {
            Utility.ValidateArgumentNonNull(element, "element");
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
            Utility.ValidateArgumentNonNull(properties, "properties");
            if (properties.Length == 0)
            {
                throw new ArgumentException("AtLeastOnePropertyMustBeSpecified");
            }
            int[] propertyIdArray = new int[properties.Length];
            for (int i = 0; i < properties.Length; ++i)
            {
                Utility.ValidateArgumentNonNull(properties[i], "properties");
                propertyIdArray[i] = properties[i].Id;
            }

            try
            {
                PropertyEventListener listener = new PropertyEventListener(AutomationElement.StructureChangedEvent, element, eventHandler);
                Factory.AddPropertyChangedEventHandler(
                    element.NativeElement,
                    (UIAutomationClient.TreeScope)scope,
                    CacheRequest.CurrentNativeCacheRequest,
                    listener,
                    propertyIdArray);
                ClientEventList.Add(listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static void AddStructureChangedEventHandler(AutomationElement element, TreeScope scope, StructureChangedEventHandler eventHandler)
        {
            Utility.ValidateArgumentNonNull(element, "element");
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

            try
            {
                StructureEventListener listener = new StructureEventListener(AutomationElement.StructureChangedEvent, element, eventHandler);
                Factory.AddStructureChangedEventHandler(
                    element.NativeElement,
                    (UIAutomationClient.TreeScope)scope,
                    CacheRequest.CurrentNativeCacheRequest,
                    listener);
                ClientEventList.Add(listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static bool Compare(int[] runtimeId1, int[] runtimeId2)
        {
            return Utility.Compare(runtimeId1, runtimeId2);
        }

        public static bool Compare(AutomationElement el1, AutomationElement el2)
        {
            try
            {
                return Utility.Compare(el1, el2);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static string PatternName(AutomationPattern pattern)
        {
            Utility.ValidateArgumentNonNull(pattern, "pattern");
            return Factory.GetPatternProgrammaticName(pattern.Id);
        }

        public static string PropertyName(AutomationProperty property)
        {
            Utility.ValidateArgumentNonNull(property, "property");
            return Factory.GetPropertyProgrammaticName(property.Id);
        }

        public static void RemoveAllEventHandlers()
        {
            try
            {
                Factory.RemoveAllEventHandlers();
                ClientEventList.Clear();
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static void RemoveAutomationEventHandler(AutomationEvent eventId, AutomationElement element, AutomationEventHandler eventHandler)
        {
            Utility.ValidateArgumentNonNull(element, "element");
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
            Utility.ValidateArgument(eventId != AutomationElement.AutomationFocusChangedEvent, "Use FocusChange notification instead");
            Utility.ValidateArgument(eventId != AutomationElement.StructureChangedEvent, "Use StructureChange notification instead");
            Utility.ValidateArgument(eventId != AutomationElement.AutomationPropertyChangedEvent, "Use PropertyChange notification instead");

            try
            {
                BasicEventListener listener = (BasicEventListener)ClientEventList.Remove(eventId, element, eventHandler);
                Factory.RemoveAutomationEventHandler(eventId.Id, element.NativeElement, listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static void RemoveAutomationFocusChangedEventHandler(AutomationFocusChangedEventHandler eventHandler)
        {
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

            try
            {
                FocusEventListener listener = (FocusEventListener)ClientEventList.Remove(AutomationElement.AutomationFocusChangedEvent, null, eventHandler);
                Factory.RemoveFocusChangedEventHandler(listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static void RemoveAutomationPropertyChangedEventHandler(AutomationElement element, AutomationPropertyChangedEventHandler eventHandler)
        {
            Utility.ValidateArgumentNonNull(element, "element");
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");
            
            try
            {
                PropertyEventListener listener = (PropertyEventListener)ClientEventList.Remove(AutomationElement.AutomationPropertyChangedEvent, element, eventHandler);
                Factory.RemovePropertyChangedEventHandler(element.NativeElement, listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public static void RemoveStructureChangedEventHandler(AutomationElement element, StructureChangedEventHandler eventHandler)
        {
            Utility.ValidateArgumentNonNull(element, "element");
            Utility.ValidateArgumentNonNull(eventHandler, "eventHandler");

            try
            {
                StructureEventListener listener = (StructureEventListener)ClientEventList.Remove(AutomationElement.StructureChangedEvent, element, eventHandler);
                Factory.RemoveStructureChangedEventHandler(element.NativeElement, listener);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }
    }
}
