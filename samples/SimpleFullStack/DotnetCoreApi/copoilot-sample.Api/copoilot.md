# Copoilot chat
## Ask mode in Copilot
1. Explain the code: 
	```
	@workspace , Explain the DataAccess project in the solution. Also explain how is it being used and triggerred
	```
2. Ask copoilot to implement a feature:
	```
	 @workspace, In ProductService class, AddProductAsync method, add a check to see if the product already exists in the database. If it does, throw a custom exception named ProductAlreadyExistsException.
	```
3. Ask copilot to implement a feature:
	```
	@workspace , Create a controller for ProductReviews CRUD operations and add a service named ProductReviewService that implements IProductReviewService interface to handle db operations and add it to DI. 
	Also add necessary Dto models. 
	```
4. Ask copoilot about an exception:
	```
	Run GetProducts end point and check if there are any exceptions, and explore "Analyze with copoilot"
	```

## Edit Mode in Copoilot
1. Document the code:
	```
	@workspace , Document the productAttributeService class and its methods. 
	```
2. Implement a feature: 
	 ```
	@workspace , Create a controller for ProductReviews CRUD operations and add a service named ProductReviewService that implements IProductReviewService interface to handle db operations and add it to DI. 
	Also add necessary Dto models. 
	```
3. Create unit tests for the code:
	```
	@workspace , Create unit tests for the ProductAttributeService class and its methods. 
	```

# Inline chat with copoilot
1. Explain about a selected code:

2. Ask copoilot to write code:
	```
	In ProductService class, AddProductAsync method, select the method body and click "Alt + /" to open inline chat.
	Please add a check if teh Category already exists in the database. If it does, throw a exception.
	``` 

	