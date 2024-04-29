# DataverseAIFunctions

https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/plugin-development
https://docs.microsoft.com/en-us/powerapps/developer/data-platform/plug-ins

- Create Project: https://docs.microsoft.com/en-us/powerapps/developer/data-platform/tutorial-write-plug-in#create-a-visual-studio-project-for-the-plug-in
    - .NET Framework 4.6.2
    - Get latest CrmSdk.CoreAssemblies
- Sign Plugin: https://docs.microsoft.com/en-us/powerapps/developer/data-platform/tutorial-write-plug-in#sign-the-plug-in
  
# Plugin Registration Tool
- Install: https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/download-tools-nuget
- Register Plugin: https://docs.microsoft.com/en-us/powerapps/developer/data-platform/tutorial-write-plug-in#register-plug-in
  - Isolation Mode is always 'Sandbox' in D365
  - Location is always 'Database' in D365
  - Signd dll is required to register successfully
- Add new Step to registered plugins
  - It tells the trigger message, and on what entity
