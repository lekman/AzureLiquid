﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AzureLiquid.Tests.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Templates {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Templates() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AzureLiquid.Tests.Resources.Templates", typeof(Templates).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;CATALOG&gt;
        ///	&lt;CD&gt;
        ///		&lt;TITLE&gt;Empire Burlesque&lt;/TITLE&gt;
        ///		&lt;ARTIST&gt;Bob Dylan&lt;/ARTIST&gt;
        ///		&lt;COUNTRY&gt;USA&lt;/COUNTRY&gt;
        ///		&lt;COMPANY&gt;Columbia&lt;/COMPANY&gt;
        ///		&lt;PRICE&gt;10.90&lt;/PRICE&gt;
        ///		&lt;YEAR&gt;1985&lt;/YEAR&gt;
        ///	&lt;/CD&gt;
        ///	&lt;CD&gt;
        ///		&lt;TITLE&gt;Hide your heart&lt;/TITLE&gt;
        ///		&lt;ARTIST&gt;Bonnie Tyler&lt;/ARTIST&gt;
        ///		&lt;COUNTRY&gt;UK&lt;/COUNTRY&gt;
        ///		&lt;COMPANY&gt;CBS Records&lt;/COMPANY&gt;
        ///		&lt;PRICE&gt;9.90&lt;/PRICE&gt;
        ///		&lt;YEAR&gt;1988&lt;/YEAR&gt;
        ///	&lt;/CD&gt;
        ///	&lt;CD&gt;
        ///		&lt;TITLE&gt;Greatest Hits&lt;/TITLE&gt;
        ///		&lt;ARTIST&gt;Dolly Parton&lt;/ARTIST&gt;
        ///		&lt;COUNTRY&gt;USA&lt;/COUNTR [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AlbumsContent {
            get {
                return ResourceManager.GetString("AlbumsContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///  {
        ///    &quot;artist&quot;: &quot;Bob Dylan&quot;,
        ///    &quot;title&quot;: &quot;Empire Burlesque&quot;
        ///  },
        ///
        ///  {
        ///    &quot;artist&quot;: &quot;Bonnie Tyler&quot;,
        ///    &quot;title&quot;: &quot;Hide your heart&quot;
        ///  },
        ///
        ///  {
        ///    &quot;artist&quot;: &quot;Dolly Parton&quot;,
        ///    &quot;title&quot;: &quot;Greatest Hits&quot;
        ///  }
        ///].
        /// </summary>
        internal static string AlbumsResult {
            get {
                return ResourceManager.GetString("AlbumsResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {% assign albums = content.CATALOG.CD -%}
        ///[{%- for album in albums limit:3 %}
        ///  {
        ///    &quot;artist&quot;: &quot;{{ album.ARTIST }}&quot;,
        ///    &quot;title&quot;: &quot;{{ album.TITLE}}&quot;
        ///  }{% if forloop.last == false %},{% endif %}
        ///  {%- endfor -%}
        ///].
        /// </summary>
        internal static string AlbumsTemplate {
            get {
                return ResourceManager.GetString("AlbumsTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///&quot;format&quot;: &quot;F4860&quot;,
        ///&quot;storeid&quot;: &quot;123&quot;,
        ///&quot;sequence&quot;: 100
        ///}.
        /// </summary>
        internal static string Append100Content {
            get {
                return ResourceManager.GetString("Append100Content", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///&quot;output&quot;: &quot;F4860123.101&quot;
        ///}.
        /// </summary>
        internal static string Append100Expected {
            get {
                return ResourceManager.GetString("Append100Expected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {% assign sequence = content.sequence | Plus: 1 -%}
        ///{% if sequence &gt; 999 -%}
        ///{% assign sequence = 1 -%}
        ///{% endif -%}
        ///{% assign label = sequence | Prepend: &apos;00&apos; -%}
        ///{% assign len = label | Size -%}
        ///{% assign start = len | Minus: 3 -%}
        ///{% assign label = label | Slice:start,len -%}
        ///{% assign label = content.format | Append: content.storeid | Append: &apos;.&apos; | Append: label -%}
        ///{
        ///&quot;output&quot;: &quot;{{ label }}&quot;
        ///}.
        /// </summary>
        internal static string AppendTemplate {
            get {
                return ResourceManager.GetString("AppendTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;contentSource&quot;: &quot;warehouse/uk.ldn.15/availability&quot;,
        ///  &quot;data&quot;: {
        ///    &quot;id&quot;: &quot;IXP89373722/0/10&quot;,
        ///    &quot;inTransit&quot;: 7,
        ///    &quot;lastModified&quot;: &quot;2022-10-04T08:17:44.7148833Z&quot;,
        ///    &quot;quantityReserved&quot;: 0,
        ///    &quot;sellableStock&quot;: 107
        ///  },
        ///  &quot;dataSchema&quot;:
        ///    &quot;https://github.com/lekman/AzureLiquid/blob/main/Schemas/foo.schema.json&quot;,
        ///  &quot;dataVersion&quot;: &quot;1.0&quot;,
        ///  &quot;eventType&quot;: &quot;Warehouse.Stock.Updated&quot;,
        ///  &quot;metadataVersion&quot;: &quot;1.0&quot;,
        ///  &quot;spanId&quot;: &quot;08585367190449175535730254221CU00&quot;,
        ///  &quot;subject&quot;: &quot;public&quot;,
        ///  &quot;trac [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EventContent {
            get {
                return ResourceManager.GetString("EventContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;available&quot;: 107,
        ///  &quot;id&quot;: &quot;IXP89373722/0/10&quot;
        ///}.
        /// </summary>
        internal static string EventResult {
            get {
                return ResourceManager.GetString("EventResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;available&quot;: {{ content.data.sellableStock }},
        ///  &quot;id&quot;: &quot;{{ content.data.id }}&quot;
        ///}.
        /// </summary>
        internal static string EventTemplate {
            get {
                return ResourceManager.GetString("EventTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;
        ///    &lt;h1&gt;Simple Template&lt;/h1&gt;
        ///&lt;/p&gt;.
        /// </summary>
        internal static string SimpleResult {
            get {
                return ResourceManager.GetString("SimpleResult", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p&gt;
        ///    &lt;h1&gt;{{ content.Title }}&lt;/h1&gt;
        ///&lt;/p&gt;.
        /// </summary>
        internal static string SimpleTemplate {
            get {
                return ResourceManager.GetString("SimpleTemplate", resourceCulture);
            }
        }
    }
}
