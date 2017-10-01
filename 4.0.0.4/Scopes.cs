using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLive.Scopes
{
    public static class WL
    {
        public const string Basic = "wl.basic";
        public const string OfflineAccess = "wl.offline_access";
        public const string SignIn = "wl.signin";

        public const string Birthday = "wl.birthday";
        public const string Calendars = "wl.calendars";
        public const string CalendarsUpdate = "wl.calendars_update";
        public const string ContactsBirthday = "wl.contacts_birthday";
        public const string ContactsCreated = "wl.contacts_create";
        public const string ContactsCalendars = "wl.contacts_calendars";
        public const string ContactsPhotos = "wl.contacts_photos";
        public const string ContactsSkyDrive = "wl.contacts_skydrive";
        public const string Emails = "wl.emails";
        public const string EventsCreate = "wl.events_create";
        public const string IMAP = "wl.imap";
        public const string PhoneNumbers = "wl.phone_numbers";
        public const string Photos = "wl.photos";
        public const string PostalAddresses = "wl.postal_addresses";
        public const string SkyDrive = "wl.skydrive";
        public const string SkyDriveUpdate = "wl.skydrive_update";
        public const string WorkProfile = "wl.work_profile";
    }
    public static class Office
    {
        public const string OneNoteCreate = "office.onenote_create";
    }
}

namespace WLive
{
    [Flags]
    public enum FilterTypes
    {
        None=0, Album=1, Audio=2, Folder=4, Photo=8, Video=16, Notebook=32
    }
    public enum OverwriteOption
    {
        Overwrite, DoNotOverwrite, ChooseNewName
    }

    public static class WLType
    {
        public const string Photo = "PHOTO";
        public const string Audio = "AUDIO";
        public const string Notebook = "NOTEBOOK";
        public const string File = "FILE";
        public const string Folder = "FOLDER";
        public const string Album = "ALBUM";

        public static bool IsFile(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return string.Compare(value.ToUpper(CultureInfo.InvariantCulture), Photo, StringComparison.Ordinal) == 0
                || string.Compare(value.ToUpper(CultureInfo.InvariantCulture), Audio, StringComparison.Ordinal) == 0
                || string.Compare(value.ToUpper(CultureInfo.InvariantCulture), Notebook, StringComparison.Ordinal) == 0
                || string.Compare(value.ToUpper(CultureInfo.InvariantCulture), File, StringComparison.Ordinal) == 0;
        }

        public static bool IsFolder(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return string.Compare(value.ToUpper(CultureInfo.InvariantCulture), Folder, StringComparison.Ordinal) == 0
                || string.Compare(value.ToUpper(CultureInfo.InvariantCulture), Album, StringComparison.Ordinal) == 0;
        }
        
}
}
