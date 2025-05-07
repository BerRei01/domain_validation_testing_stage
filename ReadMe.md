# Purpose
This is a demonstration of using different approaches to achieve a valid domain object "Meeting", two business rules are enforced in this demonstration. Practicality and ease of use were a focus of this investigation.
A simple infrastructure layer and API/Presentation layer were added for completeness. There is a number of test cases, ensuring faulty domain objects are detected with each different approach.

# How to run
Build the solution
Run the discovered tests
Use the http Launch profile, run the solution
Talk to the api, see test.MeetingController.http file as a reference

# Dependencies
ErrorOr package was chosen as an available and feature rich Result Type for the error propagation (from Application layer to Presentation) and in some cases for the validation itself.
ErrorOr's error type was used to store the user readable error messages for most approaches, a proof of concept for localized error messages was developed on top.
ErrorOr also comes with some functional paradigms (.Then() & .ThenDo()) that were part of this investigation and further expanded on (.ValidateAny() and .ValidateAnyDo()).
ErrorOr was also extended in the regards to exception conversion and flattening of List<ErrorOr<T>>.
MediatR was chosen to implement the request handlers but is not required for the concepts to work.

# Notable code
Search for the comment "// NOTABLE:" to quickly navigate to interesting bits and pieces

# Notable files
- The entire domain layer, it contains 4 different validated takes on the Meeting object (**SetMeeting.cs, ErrorOrMeeting.cs, AttributeMeeting.cs, and ValueMeeting.cs**)
- The 4 MediatR handlers that take incoming requests and validate the resulting for each of the 4 Meeting flavors, each handler has multiple methods for validating which are chosen at random (**Src/Application/CreateMeeting/Implementation**)
- **Tests/Tests.Integration/MeetingControllerIntegrationTests.cs** which simulates valid and invalid attempts to create Meetings via the API, it runs multiple times to account for the handlers' randomness
- **Tests/Tests.UnitTests/ValidationObjectsTests.cs** tests some of the more fundamental aspects of the concepts under investigation, like localization and the functional chains.
- **Src/Presentation/Controllers/MeetingController.cs** defines the endpoints for the creation of  4 Meeting flavors and error exposing
- **Src/Presentation/test.MeetingController.http** for playing with manual requests against the endpoints

# Concepts investigated
A list of covered concepts in this project

## Rules Storage / Management
- attributes DataAnnotations  ❌ -> **DataAnnotationMeeting.cs**
- in method ✅ -> **SetMeeting.cs & ErrorOrMeeting.cs & ValueMeeting.cs**
- attributes DataAnnotations extension ❌ -> **DataAnnotationMeeting.cs**
- Fluent Validation with class attribute ✅ -> **AttributeMeeting.cs & ValidAttributeMeeting**

## Return types
- void -> **SetMeeting.cs & ValueObject.cs** ✅
- property -> none -> **AttributeMeeting.cs** ✅
- ErrorOr -> **ErrorOrMeeting.cs & ValueObject.cs** ✅


## Batch Validation Methods
- Procedural step-by-step -> **CreateErrorOrMeetingRequestHandler.cs/Method3**
- Procedural list -> **CreateErrorOrMeetingRequestHandler.cs/Method4**
- Functional -> The others
- try-catch Block -> **CreateSetMeetingRequestHandler.cs/Method3 & CreateAttributeMeetingRequestHandler.cs/Method2 CreateValueMeetingRequestHandler.cs/Method3**

## Error Propagation
- just ErrorOr

## Check for Proof of Concept
- Functional Validation chain is broken by exception ✅
- Localization is possible with ErrorOr  ✅ -> **DomainError.cs & LocalizedErrors.cs**
- Exception validation details (faulty member names) are not lost with ErrorOr ✅
- Convert ErrorOr to Controller Result is easy ✅
- Valid state is preserved, object remains valid ✅

# Lessons learned
- Order of access becomes very relevant with "Always Validify entire object"

# Further investigation
.Net 10 with C# 14 will feature extension members which could be an interesting way to add validation to fields and properties.