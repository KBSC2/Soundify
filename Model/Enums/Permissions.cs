namespace Model.Enums
{
    // use number of enum to refer to db id
    public enum Permissions
    {
        SongShuffle = 1,
        SongNext,
        SongPrev,
        SongLoop,
        SongUpload,
        SongEditOwn,
        SongEditAll,
        SongDeleteOwn,
        SongDeleteAll,
        SongConfirm,
        PlaylistCreate,
        PlaylistLimit,
        PlaylistRename,
        PlaylistSongsLimit,
        AccountUsernameChange,
        AccountChangeRole,
        AccountEditOwn,
        AccountEditAll,
        AdminDashboard,
    }
}