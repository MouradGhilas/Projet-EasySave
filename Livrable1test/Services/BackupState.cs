namespace Livrable1.Services
{
    public class BackupState
    {
        public string BackupName { get; set; } = string.Empty;
        public DateTime LastActionTimestamp { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalFiles { get; set; }
        public long TotalSize { get; set; }
        public int RemainingFiles { get; set; }
        public long RemainingSize { get; set; }
        public string? CurrentSourceFile { get; set; }
        public string? CurrentTargetFile { get; set; }
    }
}