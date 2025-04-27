# FlexaNG

![FlexaNG Logo](flexa_logo.png)

## Next generation of flexa, incredible easy tool for device information grabbing

FlexaNG is an open-source diagnostic utility designed to collect extensive system information for troubleshooting, analysis, and educational purposes. The application provides a simple yet powerful interface to gather detailed hardware and software information from Windows systems.

## 🚀 Features

### Core Functionality
- **Comprehensive System Information Collection**: Gathers detailed data about hardware components, operating system configuration, and installed software
- **Browser Data Analysis**: Optional collection of browser profiles data (including history, bookmarks, and other browser-related information)
- **File System Mapping**: Option to generate a complete directory tree of the C: drive
- **Data Compression**: Built-in capability to compress all collected logs into a single ZIP file
- **Portable Design**: Runs without installation, making it ideal for technicians and system administrators

### Information Collected
FlexaNG collects a wide range of system information, including but not limited to:

#### Hardware Information
- BIOS details and configuration
- CPU specifications
- RAM configuration
- Graphics card information
- Storage devices and partitions
- Motherboard details

#### System Configuration
- Operating system details
- Installed software
- System services
- Device drivers
- User accounts
- Network configuration

#### Network Information
- IP configuration
- Network adapters
- Active connections
- DNS cache

#### System Status
- Running processes
- Power configuration
- Scheduled tasks
- System environment variables

## 📋 Screenshots

![flexa_screen](https://github.com/user-attachments/assets/f0201ca1-fb4f-4d41-8333-25926abb7283)

## 📋 Usage

1. **Launch the Application**: Run FlexaNG.exe
2. **Configure Options**:
   - Check "Compress the log folder into a ZIP file" to create a compressed archive
   - Check "Allow to read web browsers data" to collect browser profiles information
   - Check "Make TREE of entire C: drive" to generate a directory structure map
3. **Click "Proceed"**: The application will begin collecting information
4. **View Results**: All collected data is saved to a folder on your desktop named "FlexaNG_logs_[ComputerName]_[DateTime]"

## 🔒 Privacy and Security

FlexaNG is designed with privacy in mind:
- Browser data collection is **opt-in only** and requires explicit user consent
- All data remains on the local machine unless manually shared
- No data is transmitted over the internet
- The application is completely transparent about what information it collects

## 📝 License

This project is licensed under the [GNU General Public License v3.0] - see the LICENSE file for details.

## 🤝 Contributing

Contributions, issues and feature requests are welcome!
Feel free to check the [issues page](https://github.com/000rosiu/FlexaNG/issues).

## ⭐ Show your support

Give a ⭐️ if this project helped you!

## 🔧 Technical Details

- **Platform**: Windows with installed .NET Framework (min. 4.7.2)
- **Frameworks**: .NET C#
- **Dependencies**: Requires 7z in the application directory
- **Output Format**: Plain text log files (if browser information grabbing feature enabled: entire profile folders)
