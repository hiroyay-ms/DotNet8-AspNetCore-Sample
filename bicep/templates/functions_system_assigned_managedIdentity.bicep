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


module functionFlexConsumption '../modules/functions_flex_consumption_sami.bicep'= {
  name: 'functionFlexConsumption'
  params: {
    storageAccountName: storageAccountName
    newOrExisting: newOrExisting
    workspaceName: workspaceName
    appInsightsName: appInsightsName
    planName: planName
    functionName: functionName
    location: location
  }
  dependsOn: [
    storageAccount
  ]
}
