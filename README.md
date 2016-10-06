## CSpec ##

####nuget packages
- nunit: Install-Package cspec
- mstest: Install-Package cspec.mstest
 
**CSpec** is an acceptance test framework for C#. It provides two things that teams may prefer over specflow or other bdd frameworks.

- Use the full power of your favourite c# refactoring tools
- Keep the documentation clean

### How is this achieved? ###

It's all C#, and it's not driven by gherkin! It's simpler. It's bullet points to define the acceptance criteria. 

With CSpec, Given When Then (GWT) is still relevant, but it isn't the first class citizen that it is with other frameworks. 

Let's face it, anyone who has worked with Given When Then knows it is expressive, but it is also clunky, repetitive, and noisy. If you look at documentation output, it's far too verbose. 

A common scenario I have encountered is when you take a feature file with given when then in it and you find that things are worded as slightly the wrong level, there is either too much or too little detail for the automation, so you decide to change it. But changing it means changing it in every block (the scenarios), then you want to slightly reword something else and it's the same e problem again. It just gets messy.

The business owner just wanted to describe the feature, they don't want all the details of all the test cases and scenarios, just confidence that it's covered.

#There is another way

Let the testers and developers be testers and developers s and take responsibility for the scenarios that cover simply bullet points defined with the product owner or BA.

#Take this example

In order to keep my data private
As a customer
I want any personal details behind a login area

Story is complete when 
- there is a login box where a signup up user can log in
- all errors with login give generic message of 'invalid login attempt'
- successful login takes user to the dashboard

This definition of the features is succint and accurate, why deviate from it?

This is why I made cspec, to keep business level conversations with the busines and keep Given When Then with the coders and the testers. 

#But where's my Given When Then ? 

Don't worry,it's still GWT, you just write it in refactorable code, and the documentation is generated rather that it being the starting point



I love cucumber with it's simplicity of the business language features files in plain text and then the ability to write steps that run directly off the plain text file. Specflow lacks the elegance ruby allows cucumber to have. Instead it relies (very effectively) on an intermediate translation layer. I just get frustrated when I want to change a step name because I have to rename it in 3 places manually rather than pressing F2 with resharper.

This upfront of defining the steps in too much detail prior to automating is the cause of the problem rather than the tooling.

##The real world don't talk GWT to talk, so why do we do it in software ?

Because it is expressive! That's why we still need it, we just need it a fraction later in the process.

CSpec is built with a specific process in mind and that process is bullet points withe product owners, Scenarios with developers and testers 

#Show me the code

So, your tests are no different to what you know now, they are simple NUnit Tests. With one addition, a Description attribute to show which bullet point from the features that you are covering with that test (or tests)

    public class LoginTests : ExampleFeature
    {
         [Test(Description="there is a login box where a signed up user can log in")]
         public void login_box_is_on_the_homepage_as_popup()
         {
             given_i_am_on_the_homepage();
             when_i_click_sign_in();
             then_i_am_shown_a_popup_where_i_can_login();
         }
    }

This *MUST* inherit from the Feature marker class. This is to enforce test organisation rather than for true inheritance.

The marker class has the attributes that make this recognised as a feature as well as the actual documentation.

     using Cspec.Framework;
     [In order to("keep my data private")]
     [As a("customer")]
     [I want("I want any personal data behind a login area")]
 
     [Criteria(new []{ 
       "there is a login box where a signed up user can log in",
       "all errors with login give generic message of 'invalid login attempt'",
       "successful login takes user to the dashboard"
     })]

     public class ExampleFeature
     {
        // marker class for the feature
     }

This is taken a stage further and you can omit the Description from the Test definitions if you place them in a file with the same name (CamelCase or snake case) as the criteria, eg.

     public class there_is_a_login_box_where_a_signed_up_user_can_log_in : ExampleFeature
     {
        [Test]
        public void_login_box_is_on_the_homepage_as_popup()
        {
           given_i_am_on_the_homepage();
           when_i_click_sign_in();
           then_i_am_shown_a_popup_where_i_can_login();
        }
     }

Which now makes it a very obvious place for a second scenario - I like to put these classes in a folder called scenarios under each feature folder.

That's all there is to it - Write your Tests like normal, but use basic Given When Then methods inside the test (because these contribute to the documention - and put the actually nitty gritty code inside there.

*That's all there is to it*

Well, almost. Add a test somewhere with the following code and stick it in the build.

     new GherkinFeatureGenerator().Build();
     //or
     new HtmlFeatureGenerator().Build();

If using a unit test to generate the report, it needs to know where the source files are as it doesn't use reflection, so if they aren't in \Features from the acceptance test project, then you'll need to provide this path in the app.config for that project

    <appSettings>
      <add key="featureFilesRootPath" value="relative path from output folder here"/>
    </appSettings>

#Continuous Integration Server

There is also an exe provided in the /runner folder with the nuget package - this is really useful for running on a build server like Teamcity - you can then add a report tab for the output report and now your documentation is live with your builds.

    C:\Projects\Tenkai\cspec\Cspec.Generator\bin\Release>Cspec.Generator.exe
    Usage:
     cspec.generator.exe <report type> <assembly file path> <features folder location> <output file path>
    eg. 
     cspec.generator.exe HTML bin\release\AcceptanceTests.dll \Features Docs\Features.html


   

