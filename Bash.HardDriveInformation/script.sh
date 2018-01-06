sudo hdparm -I /dev/sda |
grep -P "(Firmware Revision:|Serial Number:|Transport:|Model Number:|^\tSupported:|DMA:|PIO:)" |
awk '
function clean_tabs(s) {
    # Deleting unnecessary tabulation 
    sub(/^[ \t]+/, "", s);
    return s;
}
# We need to clean only our parameters, one by one
{print clean_tabs($0);};
'
df /dev/sda? |
awk '
{
    if (NR > 1 && substr($1, 0, 5) != "udev") {
        all_space += $2;
        used_space += $3;
        free_space += $4;
    }
}
END {
    print "Total size: " all_space;
    print "Used: " used_space;
    print "Available: " free_space;
    print "Usage: " used_space / all_space * 100 "%";
}
'
