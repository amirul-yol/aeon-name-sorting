# AEON Name Sorting

Simple Windows Forms app to sort cardholder data files by name.

## How it Works

1. Click "Select Files" to choose your .txt files
2. App will sort the files by name (alphabetically)
3. Sorted files are saved in:
   - `mailer/` folder for files with "_M_N_" in name
   - `emboss/` folder for all other files
4. Output files have "-sorted" added to their names

## Notes
- Name field starts at column 61 in each line
- Original data format is preserved
- Files are sorted case-insensitive