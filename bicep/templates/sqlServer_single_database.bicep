param newOrExistingServer string
param sqlServerName string
param aadAdminName string
param aadAdminId string
param dataSource string
param databaseName string
param tier string
param skuName string
param family string
param capacity int
param maxSizeBytes int
param requestedBackupStorageRedundancy string
param collation string
param location string

module sqlServer '../modules/sqlServer_aad_login.bicep' = if (newOrExistingServer == 'new') {
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

module empty_database '../modules/sqlDatabase.bicep' = if (dataSource == 'new') {
  name: 'empty_database'
  params: {
    sqlServerName: sqlServerName
    databaseName: databaseName
    tier: tier
    skuName: skuName
    family: family
    capacity: capacity
    maxSizeBytes: maxSizeBytes
    requestedBackupStorageRedundancy: requestedBackupStorageRedundancy
    collation: collation
    location: location
  }
  dependsOn: [
    sqlServer
  ]
}
