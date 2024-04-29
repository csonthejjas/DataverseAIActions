const getAiTranslateRequest = (text, targetLanguage) => ({
    Text: text,
    TargetLanguage: targetLanguage,
    getMetadata: () => ({
        boundParameter: null,
        parameterTypes: {
            Text: { 
                typeName: "Edm.String", 
                structuralProperty: 1
            },
            TargetLanguage: {
                typeName: "Edm.String",
                structuralProperty: 1
            }
        },
        operationType: 0, // This is an action
        operationName: "AITranslate"

    })
});

const callAITranslate = async (text, targetLanguage) => {
    const response = await Xrm.WebApi.online.execute(getAiTranslateRequest(text, targetLanguage))
    if (!response.ok) {
        alert(`Error: ${response.statusText}`);
    }
    const result = await response.json();
    console.log({result});
    return result.TranslatedText;   
}

async function onLoad(context) {
    const toTranslate = "Ez egy mondat amit le kell fordítani angolra.";
    const translation = await callAITranslate(toTranslate, "en")
        
    const alertStrings = { 
        title: `Translate: ${toTranslate}`,
        text: translation
    };
    Xrm.Navigation.openAlertDialog(alertStrings, { width: 350 })
}