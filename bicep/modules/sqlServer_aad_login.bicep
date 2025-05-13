param sqlServerName string
param aadAdminName string
param aadAdminId string
param location string

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: aadAdminName
      sid: aadAdminId
      tenantId: subscription().tenantId
    }
  }
}
