import React, { createContext, useState } from "react";
import { apiUrl } from "../utils/api_url";
import { sendNotification } from "../Pages/PushNotification";

export const ContextPage = createContext();

export default function ContextProvider(props) {
  const [email, setEmail] = useState();
  const [phone, setPhone] = useState();
  const [userName, setUserName] = useState();
  const [password, setPassword] = useState();
  const [confirm, setConfirm] = useState();
  const [loginUser, setLoginUser] = useState();

  const [emailB, setEmailB] = useState();
  const [phoneB, setPhoneB] = useState();
  const [nameB, setNameB] = useState();
  const [address, setAddress] = useState();
  const [city, setCity] = useState();
  const [foodTypeB, setFoodTypeB] = useState();
  const [imgB, setImgB] = useState();
  const [passwordB, setPasswordB] = useState();
  const [confirmB, setConfirmB] = useState();

  const [users, setUsers] = useState([]);
  const [foodTypes, setFoodTypes] = useState([]);
  const [restaurants, setRestaurants] = useState([]);
  const [filteredRestaurants, setFilteredRestaurants] = useState([]);

  const [location, setLocation] = useState();
  const [errorMsg, setErrorMsg] = useState();
  const [foodType, setFoodType] = useState();

  const [foodListVisible, setFoodListVisible] = useState(false);
  const [isLoading, setIsLoading] = useState(false);


  const LoadUsers = async () => {
    try {
      let res = await fetch(`${apiUrl}/api/users`);
      let data = await res.json();
      if (data) {
        setUsers(data);
      }
    } catch (error) {
      console.log({ error } );
    }
  };

  const LoadFoodTypes = async () => {
    try {
      let res = await fetch(`${apiUrl}/api/foodTypes`);
      let data = await res.json();
      if (data) {
        setFoodTypes(data);
      }
    } catch (error) {
      console.log({ error });
    }
  }

  const LoadRestaurants = async () => {
    try {
      let res = await fetch(`${apiUrl}/api/restaurants`);
      let data = await res.json();
      if (data) { 
        setRestaurants(data);
      }
    } catch (error) {
      console.log({ error });
    } finally {
      LoadRestaurants();
    }
  }

  const checkEmail = async (email) => {
    try {
      let res = await fetch(`${apiUrl}/api/users/email?Email=${encodeURIComponent(email)}`);
      if (res.ok) {
        return true;
      } else {
        return false;
      }
    } catch (error) {
      return error;
    }
  };

  const checkEmailBusiness = async (email) => {
    try {
      let res = await fetch(`${apiUrl}/api/restaurants/email?Email=${encodeURIComponent(email)}`);
      if (res.ok) {
        return true;
      } else {
        return false;
      }
    } catch (error) {
      return error;
    }
  };

  const checkLoginUser = async (email, password) => {
    try {
      let res = await fetch(`${apiUrl}/api/users/login`, {
        method: "POST",
        body: JSON.stringify({ email, password }),
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (res.ok) {
        let data = await res.json();
        console.log(data);
        if (data) {
          return data;
        } else {
          return null;
        }
      }
    } catch (error) {
      console.error(error);
    } finally {
      LoadUsers();
    }
  };
  
  const checkLoginRestaurant = async (email, password) => {
    try {
      let res = await fetch(`${apiUrl}/api/restaurants/login`, {
        method: "POST",
        body: JSON.stringify({email, password}),
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (res.ok) {
        let data = await res.json();
        console.log(data);
        if (data) {
          return data;
        } else {
          return null;
        }
      }
    } catch (error) {
      console.error(error);
    } finally {
      LoadUsers();
    }
  };

  const addUser = async (user) => {
    try {
      let res = await fetch(`${apiUrl}/api/users/add`, {
        method: "POST",
        body: JSON.stringify(user),
        headers: {
          "Content-Type": "application/json",
        },
      });
      let data = await res.json();
      console.log(data);
    } catch (error) {
      console.error(error);
    } finally {
      LoadUsers();
    }
  };

  const deleteUser = async (id) => {
    try {
      let res = await fetch(`${apiUrl}/api/users/delete/${id}`, {
        method: "DELETE",
      });
      let data = await res.json();
      console.log(data);
    } catch (error) {
      console.error(error);
    } finally {
      LoadUsers();
    }
  };

  const addRestaurant = async (restaurant) => {
    try {
      console.log(restaurant);
      let res = await fetch(`${apiUrl}/api/restaurants/add`, {
        method: "POST",
        body: JSON.stringify(restaurant),
        headers: {
          "Content-Type": "application/json",
        },
      });
      console.log(res.status + " add");
      if (res.ok) {
        let data = await res.json();
        console.log(data);
      }
    } catch (error) {
      console.error(error);
    } finally {
      LoadRestaurants();
    }
  };

  const deleteRestaurant = async (id) => {
    try {
      let res = await fetch(`${apiUrl}/api/restaurants/delete/${id}`, {
        method: "DELETE",
      });
      let data = await res.json();
      console.log(data);
    } catch (error) {
      console.error(error);
    } finally {
      LoadRestaurants();
    }
  };

  const changeApprovedRestaurant = async (id, email) => {
    try {
      let res = await fetch(`${apiUrl}/api/restaurants/approved/${id}`, {
        method: "PUT",
      });
      let data = await res.json();
      console.log(data);
      if (res.ok) {
        console.log("approval successful");
        await sendEmail(email);
      }
    } catch (error) {
      console.error({error: error.message});
    } finally {
      LoadRestaurants();
    }
  };

  const sendEmail = async (email) => {
    try {
      let res = await fetch(`${apiUrl}/api/restaurants/sendemail`, {
        method: "POST",
        body: JSON.stringify({ email }),
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (res.status === 200) {
        let data = await res.json();
        console.log(data);
        if (data) {
          console.log('Email sent successfully');
        } 
      } else {
        console.log('Failed to send email');
      }
    } catch (error) {
      console.error(error);
    }
  };


  const findRestaurants = async (city, foodType) => {
    try {
        let res = await fetch(`${apiUrl}/api/restaurants/find`, {
            method: "POST",
            body: JSON.stringify({ city, foodType }),
            headers: {
              "Content-Type": "application/json",
            },
          });
          if (res.ok) {
            const text = await res.text();
            let data;
      
            try {
              data = await JSON.parse(text);
            } catch (error) {
              throw new Error('Invalid JSON response');
            }
            console.log(data);
            if (data) {
              setFilteredRestaurants(data);
              setIsLoading(false);
            }
            
            return data;
          } else {
            setFilteredRestaurants([]);
            setIsLoading(false);
            return null;
          }
    } catch (error) {
        console.log(error);
    }
  };


  const makeReservation = async (restEmail, userEmail) => {
    try {
      let res = await fetch(`${apiUrl}/api/restaurants/reservation`, {
        method: "POST",
        body: JSON.stringify({restEmail, userEmail}),
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (res.ok) {
        let data = await res.json();
        console.log(data);
        sendNotification('Reservation Request', 'We have sent your reservation request to the restaurant.');
      }
    } catch (error) {
      console.error(error);
    } 
  }


  const uploadFile = async (id, file) => {
    try {
      console.log(id, file);
      let res = await fetch(`${apiUrl}/api/restaurants/menu`, {
        method: 'POST',
        body: JSON.stringify({ id, file }),
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (res.ok) {
        let data = await res.json();
        console.log(data);
      } else {
        console.log(res.ok);
      }
    } catch (error) {
      console.error(error);   
    } finally {
      LoadRestaurants();
    }
  };


  const value = {
    email, setEmail,
    phone, setPhone,
    userName, setUserName,
    password, setPassword,
    confirm, setConfirm,
    addUser,
    LoadUsers,
    LoadFoodTypes,
    LoadRestaurants,
    users,
    checkEmail, 
    checkLoginUser, checkLoginRestaurant,
    location, setLocation,
    errorMsg, setErrorMsg,
    foodType, setFoodType,
    foodListVisible, setFoodListVisible,
    foodTypes,
    restaurants, setRestaurants,
    findRestaurants,
    isLoading, setIsLoading,
    filteredRestaurants, setFilteredRestaurants,
    deleteUser,
    deleteRestaurant,
    loginUser, setLoginUser,
    emailB, setEmailB,
    phoneB, setPhoneB,
    nameB, setNameB,
    address, setAddress,
    city, setCity,
    foodTypeB, setFoodTypeB,
    imgB, setImgB,
    passwordB, setPasswordB, 
    confirmB, setConfirmB,
    addRestaurant,
    checkEmailBusiness,
    changeApprovedRestaurant,
    uploadFile, sendEmail, makeReservation,
  };

  return (
    <ContextPage.Provider value={value}>{props.children}</ContextPage.Provider>
  );
}
