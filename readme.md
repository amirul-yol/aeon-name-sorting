# AEON Name Sorting

A Windows Forms application for sorting AEON card holder data files alphabetically by the cardholder's first name.

## Features

- Processes multiple files simultaneously
- Supports both emboss and mailer file formats
- Alphabetical sorting based on first name
- Preserves original line formatting and sequence numbers
- Creates separate output folders for emboss and mailer files
- Appends "-sorted" suffix to output filenames

## Supported File Formats

1. Emboss Files
   - Filename pattern: `EM011SNX_[numbers].txt`
   - Example: `EM011SNX_525629.txt`

2. Mailer Files
   - Filename pattern: `EM011SNX_M_N_[numbers].txt`
   - Example: `EM011SNX_M_N_525629.txt`

## Usage

1. Launch the application
2. Click "Select Files" button
3. Choose one or more input files
4. Wait for processing to complete
   - Files are validated
   - Names are sorted alphabetically
   - Original formatting is preserved
5. Find sorted files in output folders:
   - `emboss/` for emboss files
   - `mailer/` for mailer files

Example:
```
Input:  EM011SNX_525629.txt
Output: emboss/EM011SNX_525629-sorted.txt
```

## Requirements

- Windows operating system
- .NET Framework