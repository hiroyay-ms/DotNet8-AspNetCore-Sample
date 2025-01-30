param storageAccountName string
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
    planName: planName
    functionName: functionName
    location: location
  }
  dependsOn: [
    storageAccount
  ]
}
