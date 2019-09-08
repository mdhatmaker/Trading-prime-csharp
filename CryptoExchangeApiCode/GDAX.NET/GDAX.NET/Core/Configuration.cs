// Copyright 2016 Rick@AIBrain.org.
//
// This notice must be kept visible in the source.
//
// This section of source code belongs to Rick@AIBrain.Org unless otherwise specified, or the
// original license has been overwritten by the automatic formatting of this code. Any unmodified
// sections of source code borrowed from other projects retain their original license and thanks
// goes to the Authors.
//
// Donations and royalties can be paid via
//  PayPal: paypal@aibrain.org
//  bitcoin: 1Mad8TxTqxKnMiHuZxArFvX8BuFEB9nqX2
//  litecoin: LeUxdU2w3o6pLZGVys5xpDZvvo8DUrjBp9
//
// Usage of the source code or compiled binaries is AS-IS. I am not responsible for Anything You Do.
//
// Contact me by email if you have any questions or helpful criticism.
//
// "GDAX.NET/Configuration.cs" was last cleaned by Rick on 2016/06/18 at 2:58 PM

namespace GDAX.NET.Core {

    using System;
    using System.Windows.Forms;
    using Librainian.Controls;
    using Librainian.FileSystem;
    using Librainian.Persistence;
    using Librainian.Threading;

    public static class Configuration {
        public const String Apikey = "apikey";
        public const String Passphrase = "passphrase";
        public const String Secret = "secret";

        public static Folder MainFolder { get; } = new Folder( Environment.SpecialFolder.LocalApplicationData, nameof( GDAX ) );

        public static PersistTable<String, Object> Settings { get; } = new PersistTable<String, Object>( MainFolder, nameof( Settings ) );

        public static String Ask( String key, String question ) {
            if ( question == null ) {
                throw new ArgumentNullException( nameof( question ) );
            }
            if ( key == null ) {
                throw new ArgumentNullException( nameof( key ) );
            }
            Object value;
            while ( !Settings.TryGetValue( key, out value ) ) {
                var questionBox = new QuestionBox( question );
                var result = questionBox.ShowDialog();
                if ( result == DialogResult.OK ) {
                    Settings[ key ] = questionBox.Response;
                }
                else {
                    if ( ThreadingExtensions.IsRunningFromNUnit ) {
                        throw new NotImplementedException();
                    }
                }
            }
            return value as String;
        }
    }
}