param sqlServerName string
param aadAdminName string
param aadAdminId string
param dataSource string
param location string

module sqlServer '../modules/sqlServer_aad_login.bicep' = {
  name: 'sqlServer'
  params: {
    sqlServerName: sqlServerName
    aadAdminName: aadAdminName
    aadAdminId: aadAdminId
    location: location
  }
}

module adventureWorksLT_database '../modules/sqlDatabase_serverless_sample_AdventureWorksLT.bicep' = if (dataSource == 'sample') {
  name: 'adventureWorksLT_database'
  params: {
    sqlServerName: sqlServerName
    location: location
  }
  dependsOn: [
    sqlServer
  ]
}
