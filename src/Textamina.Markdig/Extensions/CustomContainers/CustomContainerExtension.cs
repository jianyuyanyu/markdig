﻿// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Textamina.Markdig.Parsers.Inlines;
using Textamina.Markdig.Renderers;

namespace Textamina.Markdig.Extensions.CustomContainers
{
    /// <summary>
    /// Extension to allow custom containers.
    /// </summary>
    /// <seealso cref="Textamina.Markdig.IMarkdownExtension" />
    public class CustomContainerExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipeline pipeline)
        {
            if (!pipeline.BlockParsers.Contains<CustomContainerParser>())
            {
                // Insert the parser before any other parsers
                pipeline.BlockParsers.Insert(0, new CustomContainerParser());
            }

            // Plug the inline parser for CustomContainerInline
            var inlineParser = pipeline.InlineParsers.Find<EmphasisInlineParser>();
            if (inlineParser != null)
            {
                inlineParser.EmphasisDescriptors.Add(new EmphasisDescriptor(':', 2, 2, true));
                var previousCreateEmphasisInline = inlineParser.CreateEmphasisInline;
                inlineParser.CreateEmphasisInline = (emphasisChar, strong) =>
                {
                    if (strong && emphasisChar == ':')
                    {
                        return new CustomContainerInline();
                    }
                    return previousCreateEmphasisInline?.Invoke(emphasisChar, strong);
                };
            }

            var htmlRenderer = pipeline.Renderer as HtmlRenderer;
            if (htmlRenderer != null)
            {
                if (!htmlRenderer.ObjectRenderers.Contains<HtmlCustomContainerRenderer>())
                {
                    // Must be inserted before CodeBlockRenderer
                    htmlRenderer.ObjectRenderers.Insert(0, new HtmlCustomContainerRenderer());
                }
                if (!htmlRenderer.ObjectRenderers.Contains<HtmlCustomContainerInlineRenderer>())
                {
                    // Must be inserted before EmphasisRenderer
                    htmlRenderer.ObjectRenderers.Insert(0, new HtmlCustomContainerInlineRenderer());
                }
            }
        }
    }
}