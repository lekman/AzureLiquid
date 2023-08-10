// <copyright file="LiquidParserTests.Arrangement.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Tests
// Created: 2022-10-18 07:46
// </copyright>

using AzureLiquid.Tests.Resources;

namespace AzureLiquid.Tests
{
    public partial class LiquidParserTests
    {
        /// <summary>
        /// Contains arranged values used for testing, containing mock instances and expected return values.
        /// </summary>
        private class Arrangement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Arrangement"/> class.
            /// </summary>
            public Arrangement()
            {
                Basic = new BasicObject("John Doe");
                Deep = new TemplateFact<DeepObject>
                {
                    Content = new DeepObject("Jane Doe"),
                    Template = "<p>{{content.nested.title}}</p>",
                    Expected = "<p>Jane Doe</p>"
                };

                var simple = "Simple Template";
                SimpleTemplate = new TemplateFact<BasicObject>
                {
                    Content = new BasicObject(simple),
                    Template = Templates.SimpleTemplate,
                    Expected = Templates.SimpleResult
                };

                Event = new TemplateFact<string>
                {
                    Content = Templates.EventContent,
                    Template = Templates.EventTemplate,
                    Expected = Templates.EventResult
                };

                Albums = new TemplateFact<string>
                {
                    Content = Templates.AlbumsContent,
                    Template = Templates.AlbumsTemplate,
                    Expected = Templates.AlbumsResult
                };
            }

            /// <summary>
            /// Gets the albums fact.
            /// </summary>
            public TemplateFact<string> Albums { get; }

            /// <summary>
            /// Gets the event fact.
            /// </summary>
            public TemplateFact<string> Event { get; }

            /// <summary>
            /// Gets the simple template fact.
            /// </summary>
            public TemplateFact<BasicObject> SimpleTemplate { get; }

            /// <summary>
            /// Gets the deep object fact.
            /// </summary>
            public TemplateFact<DeepObject> Deep { get; }

            /// <summary>
            /// Gets the basic object fact.
            /// </summary>
            public BasicObject Basic { get; }

            /// <summary>
            /// Basic test of an object to serialize and render.
            /// </summary>
            public class BasicObject
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="BasicObject"/> class.
                /// </summary>
                /// <param name="title">The title.</param>
                public BasicObject(string title)
                {
                    Title = title;
                }

                /// <summary>
                /// Gets or sets the title.
                /// </summary>
                /// <value>
                /// The title.
                /// </value>
                public string Title { get; }
            }

            /// <summary>
            /// Test sample for an object with nested types, ensure that deeper accessors are valid.
            /// </summary>
            public class DeepObject
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="DeepObject"/> class.
                /// </summary>
                /// <param name="title">The title.</param>
                public DeepObject(string title)
                {
                    Nested = new BasicObject(title);
                }

                // ReSharper disable once UnusedAutoPropertyAccessor.Local // used in serialization
                // ReSharper disable once MemberCanBePrivate.Local // used in serialization
                /// <summary>
                /// Gets the nested object.
                /// </summary>
                /// <value>
                /// The nested object.
                /// </value>
                public BasicObject Nested { get; }
            }
        }

        /// <summary>
        /// A fact check for a rendered template, with data content and expected result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class TemplateFact<T>
        {
            /// <summary>
            /// Gets or sets the template.
            /// </summary>
            /// <value>
            /// The template.
            /// </value>
            public string? Template { get; set; }

            /// <summary>
            /// Gets or sets the expected result.
            /// </summary>
            /// <value>
            /// The expected result.
            /// </value>
            public string? Expected { get; set; }

            /// <summary>
            /// Gets or sets the content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            public T? Content { get; set; }
        }
    }
}