﻿// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Textamina.Markdig.Parsers.Inlines;

namespace Textamina.Markdig.Extensions.Hardlines
{
    /// <summary>
    /// Extension to generate hardline break for softline breaks.
    /// </summary>
    /// <seealso cref="Textamina.Markdig.IMarkdownExtension" />
    public class SoftlineBreakAsHardlineExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipeline pipeline)
        {
            // Simply modify the LineBreakInlineParser
            // TODO: We might want more options (like pandoc)
            var parser = pipeline.InlineParsers.Find<LineBreakInlineParser>();
            if (parser != null)
            {
                parser.EnableSoftAsHard = true;
            }
        }
    }
}