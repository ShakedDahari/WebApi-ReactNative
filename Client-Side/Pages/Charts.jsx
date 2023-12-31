import React, { useContext, useState } from 'react';
import { View, Text, Modal, TouchableOpacity, ActivityIndicator } from 'react-native';
import { ContextPage } from '../Context/ContextProvider';
import WebView from 'react-native-webview';

//Function to generate background colors
const generateBackgroundColors = (count) => {
  const colors = [];
  const hueStart = 180; // Starting hue value for blue-green
  const hueEnd = 240; // Ending hue value for blue-green

  for (let i = 0; i < count; i++) {
    const hue = hueStart + ((hueEnd - hueStart) * i) / (count - 3); // Distribute hues between the start and end values
    const saturation = '80%'; // Adjust the saturation value for desired effect
    const lightness = '70%'; // Adjust the lightness value for desired effect
    const color = `hsl(${hue}, ${saturation}, ${lightness})`;
    colors.push(color);
  }
  return colors;
};

export default function Charts(props) {

    const { restaurants, foodTypes } = useContext(ContextPage);
    const [selectedCity, setSelectedCity] = useState('All'); 
    const [modalVisible, setModalVisible] = useState(false);

  // Generate background colors for food types
  const backgroundColors = generateBackgroundColors(foodTypes.length);

  const cityList = ['All', ...new Set(restaurants.map(restaurant => restaurant.location))];

  const getFilteredRestaurantData = () => {
    if (selectedCity === 'All') {
      return restaurants.filter(restaurant => restaurant.approved == "true");
    }
    return restaurants.filter(restaurant => restaurant.location === selectedCity && restaurant.approved == "true");
  };

  const filteredRestaurantData = getFilteredRestaurantData();

  // Render the city dropdown
  const renderCityDropdown = () => {
    return (
      <Modal
        visible={modalVisible}
        animationType="slide"
        transparent={true}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
          <View style={{ backgroundColor: '#fff', padding: 20, borderRadius: 8 }}>
            {cityList.map(city => (
              <TouchableOpacity
                key={city}
                onPress={() => {
                  setSelectedCity(city);
                  setModalVisible(false);
                }}
              >
                <Text style={{ fontSize: 18, marginBottom: 10, fontFamily: 'eb-garamond' }}>{city}</Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>
      </Modal>
    );
  };


  // Calculate the percentage of each food type
  const foodTypeCounts = {};
  filteredRestaurantData.forEach((restaurant) => {
    const type = restaurant.foodType;
    foodTypeCounts[type] = (foodTypeCounts[type] || 0) + 1;
  });

  const totalRestaurants = filteredRestaurantData.length;
  const foodTypePercentages = foodTypes.map((type) => {
    const count = foodTypeCounts[type.name] || 0;
    const percentage = (count / totalRestaurants) * 100;
    return { type: type.name, percentage: parseFloat(percentage.toFixed(2)) };
  });


  // Prepare data for the doughnut chart
  const chartConfigFood = {
    type: 'doughnut',
    data: {
      labels: foodTypePercentages.map((item) => item.type),
      datasets: [
        {
          data: foodTypePercentages.map((item) => item.percentage),
          backgroundColor: backgroundColors,
          borderWidth: 2,
          borderColor: 'black',
        },
      ],
    },
    options: {
      plugins: {
        tooltip: {
          titleFont: {
            size: 35
          },
          bodyFont: {
            size: 25
          },
        },
        legend: {
          labels :{
              font: {
                size: 30,
              },
          },
          position: 'bottom', // Show the legend at the bottom
        },
      },
      layout: {
        padding: 20, // Add some padding to the chart area
      },
    },
  };

  const chartHTMLFood = `
    <html>
      <head>
        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
      </head>
      <body>
        <div style="width: 95%; margin: auto;">
          <canvas id="foodTypesChart"></canvas>
        </div>
        <script>
          const ctx = document.getElementById('foodTypesChart').getContext('2d');
          new Chart(ctx, ${JSON.stringify(chartConfigFood)});
        </script>
      </body>
    </html>
  `;
  

  return (
    <View style={{flex: 1}}>
      {restaurants.length === 0 ? (
         <ActivityIndicator size={100} color="#D9D9D9" />
      ) : (
        <View style={{ flex: 1 }}>
          <View style={{ flexDirection: 'row', justifyContent: 'center', alignItems: 'center' }}>
            <Text style={{fontSize: 20, fontWeight: 'bold', margin: 15, fontFamily: 'eb-garamond'}}>Filter by City:</Text>
            <TouchableOpacity onPress={() => setModalVisible(true)}>
              <Text style={{ fontSize: 16, fontFamily: 'eb-garamond-italic' }}>{selectedCity}</Text>
            </TouchableOpacity>
          </View>
          {renderCityDropdown()}
          <View>
            <Text style={{alignSelf: 'center', margin: 15, fontSize: 20, fontWeight: 'bold', fontFamily: 'eb-garamond'}}>Food Types Percentage</Text>
          </View>
          <View style={{ height: 400 }}>
            <WebView
              originWhitelist={['*']}
              source={{ html: chartHTMLFood }}
              style={{ flex: 1, backgroundColor: '#ededed' }}
            />
          </View>
        </View>
      )}
    </View>
  );
};

