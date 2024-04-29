function openCustomPageDialog(primaryControl, firstSelectedItemId, selectedEntityTypeName)
{
    console.log({firstSelectedItemId, selectedEntityTypeName});
    // Centered Dialog
    const pageInput = {
        pageType: "custom",
        name: "sfdemo_translatedialog_18a51",
        entityName: selectedEntityTypeName, 
        recordId: firstSelectedItemId 
    };
    const navigationOptions = {
        target: 2,
        position: 1,
        height: { value: 500, unit: "px" },
        width: { value: 50, unit: "%" },
        title: "Translate Content Text"
    };

    Xrm.Navigation.navigateTo(pageInput, navigationOptions)
        .then(() => {})
        .catch ((error) => {
            console.log(error.message);
            alert(error.message);
        });
}