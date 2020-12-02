namespace Model.Enums
{
    // use number of enum to refer to db id
    public enum Permissions
    {
        SongShuffle = 1,            //unlockable in shop
        SongNext,                   //unlockable in shop
        SongPrev,                   //unlockable in shop
        SongLoop,                   //unlockable in shop
        SongUpload,
        SongEditOwn,
        SongEditAll,
        SongDeleteOwn,
        SongDeleteAll,
        SongConfirm,
        PlaylistCreate,
        PlaylistLimit,              //unlockable in shop
        PlaylistRename,             //unlockable in shop
        PlaylistSongsLimit,         //unlockable in shop
        AccountUsernameChange,      //unlockable in shop
        AccountChangeRole,
        AccountEditOwn,
        AccountEditAll,
        AdminDashboard,
    }
}