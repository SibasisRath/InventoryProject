# InventoryProject
Outscal unity Advance Mat1 project

<p>
  <h3>Intro:</h3>
  This is a game feature that is present in all most every games. Gather gold and random items, perform transaction, either sell to shop or buy from shop. Your limitations are how much weight you can carry and how much gold you have to spend.
</p>

<p><h3>How to play:</h3>
  Gather Some random items and gold. buy and sell them on your way from shop to inventory and drom inventory to shop
  
  ![Screenshot (162)](https://github.com/user-attachments/assets/a686b2ef-8f93-4801-892a-4e059001bf20)
  
  ![Screenshot (163)](https://github.com/user-attachments/assets/4811d234-afee-45fb-9ebf-908c74c01253)
  
  ![Screenshot (164)](https://github.com/user-attachments/assets/73c9fe2c-371c-4427-be81-30bcf0f42bc0)
  
  ![Screenshot (158)](https://github.com/user-attachments/assets/a28061a1-9d51-4b65-b4cb-821b6b3546cd)
  
  ![Screenshot (159)](https://github.com/user-attachments/assets/560f9d97-c80a-4615-aae2-14d3321ab34c)
  
  ![Screenshot (160)](https://github.com/user-attachments/assets/d7d0c7c4-2e4a-408e-b911-46da19a9c05f)
  
  ![Screenshot (161)](https://github.com/user-attachments/assets/ac5d3175-f746-4fc2-b71a-ad8ff732e448)

</p>

<p><h3>Code Structure:</h3>
   Item Cell MVC is handled by Item Category MVC , which is handled by player inventory MVC and Shop service. There are other services. e.g. Sound service, event service and UI service. Here game service is acting as a main dependency container, which is fulfilling other services dependency requirements through DI.
Some Functions like generating pop up messages, activating and disactivating panel and UI components are handled by Action events (Observer pattern).
</p>

<p><h3>VIdeo Link:</h3>
  https://drive.google.com/file/d/11f-rUmbGOZdPaiAVGpvTR5ineFGH6s14/view?usp=sharing
</p>
