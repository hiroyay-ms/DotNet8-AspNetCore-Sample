param sqlServerName string
param databaseName string
param tier string
param skuName string
param family string
param capacity int
param collation string
param maxSizeBytes int
param requestedBackupStorageRedundancy string
param location string

resource sqlSqlServer 'Microsoft.Sql/servers@2024-05-01-preview' existing = {
  name: sqlServerName
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  parent: sqlSqlServer
  name: databaseName
  location: location
  sku: {
    name: skuName
    tier: tier
    family: family
    capacity: capacity
  }
  properties: {
    collation: collation
    maxSizeBytes: maxSizeBytes
    requestedBackupStorageRedundancy: requestedBackupStorageRedundancy
  }
}
