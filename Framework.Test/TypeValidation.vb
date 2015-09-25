Imports NUnit.Framework

<testfixture> Public Class TypeValidation


    <Test> Public Sub IgnoredDllIsNotLoaded()

        LazyFramework.TypeValidation.IgnoreAssemblies = "moq\.dll$"

        Dim test = LazyFramework.Reflection.FindAllClassesOfTypeInApplication(GetType(IORDataObject))

        Assert.Greater(test.Count, 0)

    End Sub


    <Test> Public Sub EmptyIgnoreLoadsAll()
        'LazyFramework.TypeValidation.IgnoreAssemblies = "moq\.dll$"

        Dim test = LazyFramework.Reflection.FindAllClassesOfTypeInApplication(GetType(IORDataObject))

        Assert.Greater(test.Count, 0)
    End Sub
End Class
