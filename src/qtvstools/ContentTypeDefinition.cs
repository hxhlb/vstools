/****************************************************************************
**
** Copyright (C) 2016 The Qt Company Ltd.
** Contact: https://www.qt.io/licensing/
**
** This file is part of the Qt VS Tools.
**
** $QT_BEGIN_LICENSE:GPL-EXCEPT$
** Commercial License Usage
** Licensees holding valid commercial Qt licenses may use this file in
** accordance with the commercial license agreement provided with the
** Software or, alternatively, in accordance with the terms contained in
** a written agreement between you and The Qt Company. For licensing terms
** and conditions see https://www.qt.io/terms-conditions. For further
** information use the contact form at https://www.qt.io/contact-us.
**
** GNU General Public License Usage
** Alternatively, this file may be used under the terms of the GNU
** General Public License version 3 as published by the Free Software
** Foundation with exceptions as appearing in the file LICENSE.GPL3-EXCEPT
** included in the packaging of this file. Please review the following
** information to ensure the GNU General Public License requirements will
** be met: https://www.gnu.org/licenses/gpl-3.0.html.
**
** $QT_END_LICENSE$
**
****************************************************************************/

#if VS2013
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace QtVsTools
{
    public sealed class QmlContentTypeDefinition
    {
        public const string ContentType = "Qml";

        /// <summary>
        /// Exports the Qml content type
        /// </summary>
        [Export(typeof(ContentTypeDefinition))]
        [Name(ContentType)]
        [BaseDefinition("code")]
        public ContentTypeDefinition QmlContentType { get; set; }

        /// <summary>
        /// Exports the Qml file extension
        /// </summary>
        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [ContentType(ContentType)]
        [FileExtension(".qml")]
        public FileExtensionToContentTypeDefinition QmlFileExtension { get; set; }

        /// <summary>
        /// Exports the Qml project file extension
        /// </summary>
        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [ContentType(ContentType)]
        [FileExtension(".qmlproject")]
        public FileExtensionToContentTypeDefinition QmlProjectFileExtension { get; set; }
    }
}
#endif
