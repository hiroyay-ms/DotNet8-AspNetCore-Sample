param managedIdentityName string
param storageAccountName string
param newOrExisting string
param workspaceName string
param appInsightsName string
param planName string
param functionName string
param location string

module storageAccount '../modules/storage_account.bicep' = {
  name: 'storageAccount'
  params: {
    storageAccountName: storageAccountName
    functionName: functionName
    location: location
  }
}

module managedIdentity '../modules/managed_identity.bicep' = {
  name: 'managedIdentity'
  params: {
    managedIdentityName: managedIdentityName
    location: location
  }
}

module roleAssignment '../modules/role_assignment.bicep' = {
  name: 'roleAssignment'
  params: {
    storageAccountName: storageAccountName
    managedIdentityName: managedIdentityName
  }
  dependsOn: [
    storageAccount
    managedIdentity
  ]
}

module functionFlexConsumption '../modules/functions_flex_consumption_uami.bicep'= {
  name: 'functionFlexConsumption'
  params: {
    managedIdentityName: managedIdentityName
    storageAccountName: storageAccountName
    newOrExisting: newOrExisting
    workspaceName: workspaceName
    appInsightsName: appInsightsName
    planName: planName
    functionName: functionName
    location: location
  }
  dependsOn: [
    roleAssignment
  ]
}
