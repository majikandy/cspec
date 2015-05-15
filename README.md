## CSpec ##

**CSpec** is an acceptance test framework for C#. It provides two things that teams may prefer over specflow or other bdd frameworks.

- Use the full power of your favourite c# refactoring tools
- Keep the documentation clean

### How is this achieved? ###

It's all C#, and it's not gherkin! It's simpler than that. It's bullet points to define the acceptance criteria. 

With CSpec, Given When Then (GWT) is still relevant, but it isn't the first class citizen that it is with other frameworks. 

Let's face it, anyone who has worked with Given When Then knows it is  expressive, but it is also clunky, repetitive, and noisy. If you look at documentation output, it's far too verbose. 

A common scenario I have encountered is when you take a feature file with given when then in it and you find that things are worded as slightly the wrong level, there is either too much or too little detail for the automation, so you decide to change it. But changing it means changing it in every block (the scenarios), then you want to  slightly reword something else and it's the same e problem again. It just gets messy.

The business owner just wanted to describe the feature, they don't want all the details of all the test cases and scenarios, just confidence that it's covered.

There is another way

Let the testers and developers be testers and developers s and take responsibility for the scenarios that cover simply bullet points defined with the product owner or BA.

Take this example

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

##The real world dont' use GWT to talk, so why do we do it in software ?

Because it is expressive! That's why we still need it, we just need it a fraction later in the process.

CSpec is built with a specific process in mind and that process is bullet points withe product owners, Scenarios with developers and testers 

#Show me the code

So, your tests are no different to what you know now, they are simple NUnit Tests. With one addition, a Description attribute to show which bullet point from the features that you are covering with that test (or tests)

    [Test(Description="there is a login box where a signup up user can log in")]
    public void login_box_is_on_the_homepage_as_popup()
    {
       given_i_am_on_the_homepage();
       when_i_click_sign_in();
       then_i_am_shown_a_popup_where_i_can_login();
    }

How do I oraganise this against the Feature?

Inherit that test class for a Feature marker class that has the attributes used to represent that feature.

    using Cspec.Framework;

    [In_order_to("keep my data private")]
    [As_a("customer")]
    [I_want("I want any personal data behind a login area")]
    
    [Criteria(new []{ 
        "there is a login box where a signup up user can log in",
        "all errors with login give generic message of 'invalid login attempt'",
        "successful login takes user to the dashboard"
    })]

    public class ExampleFeature
    {
        // marker class for the feature
    }

And now, like magic, documentation will be produced for you :)

Well, almost. Add a test somewhere with the following code and stick it in the build.

    new GherkinFeatureGenerator().Build();
	// or
	new HtmlFeatureGenerator().Build
