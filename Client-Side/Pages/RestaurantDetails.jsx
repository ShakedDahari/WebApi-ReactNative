import { View, Text, StyleSheet, ScrollView, Image, BackHandler } from 'react-native';
import React, { useContext, useEffect, useState } from 'react';
import { MaterialIcons } from '@expo/vector-icons';
import { ContextPage } from '../Context/ContextProvider';
import { Button } from 'react-native-paper';
import * as ImagePicker from 'expo-image-picker';
import { useFocusEffect } from '@react-navigation/native';
import Gallery from 'react-native-awesome-gallery';


export default function RestaurantDetails({ route, navigation }) {

  const { userType, restaurant } = route.params;
  const { uploadFile } = useContext(ContextPage);

  const [fileUri, setFileUri] = useState();
  const [isGalleryOpen, setIsGalleryOpen] = useState(false);
  const openGallery = () => setIsGalleryOpen(true);
  const closeGallery = () => setIsGalleryOpen(false);

  
  // Update the menuItems state when the restaurant prop changes
  useEffect(() => {
    if (restaurant && restaurant.menu) {
      setFileUri(restaurant.menu.split(','));
    } else {
      setFileUri([]); // Set an empty array when the menu is not available
    }
  }, [restaurant]);


    useFocusEffect(
      React.useCallback(() => {
        const handleBackPress = () => {
          switch (userType) {
            case 'regularUser':
              navigation.navigate('Main'); 
              return true; // Prevent the default back press behavior
            case 'restaurantOwner':
              navigation.navigate('Login'); 
              return true;
            default:
              return false;
          }
        };
  
        BackHandler.addEventListener('hardwareBackPress', handleBackPress);
  
        return () => {
          BackHandler.removeEventListener('hardwareBackPress', handleBackPress); // Cleanup: remove the event listener
        };
      }, [navigation, userType]) 
    );

  
const pickMenuImage = async () => {
  let result = await ImagePicker.launchImageLibraryAsync({
    allowsMultipleSelection: true,
    mediaTypes: ImagePicker.MediaTypeOptions.Images,
    aspect: [3, 4],
    quality: 1,
  });

  if (!result.canceled && result.assets.length > 0) {
    const selectedImages = result.assets.map((asset) => asset.uri);
    const filesString = selectedImages.join(","); // Convert the array to a comma-separated string
    const menuFilesArray = filesString.split(',');// Convert the comma-separated string to an array of file URIs
    setFileUri(menuFilesArray);
    console.log("filestring: " + filesString);
    await uploadFile(restaurant._id, filesString);
  } else {
    console.log('No images selected');
  }
};


  return (
    <View style={styles.container}>
    <ScrollView keyboardShouldPersistTaps="handled" overScrollMode='never' style={{ flex: 1 }}>
        <Image source={require("../assets/icon.png")} style={styles.icon}/>
        <Text style={styles.text}>DineInTime</Text>
        <Image source={{ uri: restaurant.image }} style={styles.image} />
    <View style={styles.upCon}>
        <Text style={styles.header}>{restaurant.name}</Text>
        <View style={{flexDirection: 'row', margin: 5}}> 
            <MaterialIcons name={'location-on'} style={styles.material} />
            <Text style={styles.font}>{restaurant.location}</Text>
        </View>
        <View style={{flexDirection: 'row', margin: 5}}> 
            <MaterialIcons name={'call'} style={styles.material} />
            <Text style={styles.font}>{restaurant.phone}</Text>
        </View>
        <View style={{flexDirection: 'row', margin: 5}}> 
            <MaterialIcons name={'mail'} style={styles.material} />
            <Text style={styles.font}>{restaurant.email}</Text>
        </View>
    </View>
    <View>
      <Text style={styles.menu}>Menu</Text>
      {userType === 'restaurantOwner' && (
        <Button mode='outlined' style={styles.btn} onPress={pickMenuImage}>Select Menu</Button>
        )}
    {fileUri && fileUri.length > 0 ? (
        <>
        <View style={{ flex: 1 }}>
          <Button mode='outlined' style={styles.btn} onPress={openGallery}>Show Menu</Button>
        {isGalleryOpen && (
        <Gallery
          style={{backgroundColor: '#aaccc6' }}
          data={fileUri.map((uri, index) => ({ uri, key: index }))}
          pinchEnabled
          initialIndex={0} 
          onScaleChangeRange={{start: 0, end: fileUri.length - 1}}
          keyExtractor={(item) => item.key}
          renderItem={({ item }) => (
            <Image
              source={{ uri: item.uri }}
              resizeMode="contain"
              style={{ flex: 1 }}
            />
            )}
          onSwipeToClose={closeGallery}
          />)}       
        </View>
        </>
      ) : (
        <Text style={[styles.itemName, { alignSelf: 'center' }]}>Menu not available</Text>
      )}
    </View>
    </ScrollView>
    </View>
  );
};

const styles = StyleSheet.create({
    container: {
      justifyContent: "center",
      width: "100%",
      height: "100%",
    },
    icon: {
      width: 100,
      height: 100,
      alignSelf: "center",
    },
    text: {
        alignSelf: "center",
        fontSize: 18,
        fontFamily: 'eb-garamond',
    },
    upCon: {
        marginHorizontal: 30,
    },
    image: {
        width: "90%",
        height: 200,
        alignSelf: 'center',
        borderRadius: 5,
    },
    header: {
        fontSize: 30,
        fontWeight: 'bold',
        fontFamily: 'eb-garamond',
        padding: 15,
        color: '#90b2ac',
    },
    menu: {
      fontSize: 30,
      marginTop: 20,
      fontFamily: 'eb-garamond',
      alignSelf: 'center',
      color: '#90b2ac',
  },
    font: {
        fontSize: 20,
        fontFamily: 'eb-garamond-italic',
    },
    material: {
        fontSize: 25,
        textAlignVertical: 'center',
        paddingHorizontal: 10,
    },
    head: {
        fontSize: 20,
        fontFamily: 'eb-garamond',
        margin: 15,
        textAlign: 'center',
    },
    itemName: {
        fontFamily: 'eb-garamond-italic', 
        margin: 3, 
        fontSize: 24,
    },
    btn: {
        height: 50,
        alignSelf: "center",
        width: "75%",
        borderWidth: 2,
        margin: 10,
    },
    imgBtn: {
        fontSize: 50,
        alignSelf: "center",
        borderColor: "#B0B0B0",
        borderWidth: 1,
        margin: 10,
        padding: 5,
    },
    radioLabel: {
      fontSize: 14,
      fontFamily: 'eb-garamond',
      marginVertical: 8,
    },
    options: {
      flexDirection: 'row',
      alignItems: 'center',
      paddingVertical: 10,
      borderBottomWidth: 1,
      borderBottomColor: '#ccc',
    },
    categoryLink: {
      fontSize: 16,
      fontWeight: 'bold',
      fontFamily: 'eb-garamond',
      color: '#333',
      paddingHorizontal: 10,
    },
    activeLink: {
      color: '#90b2ac', // Change the color for the active link
    },
    section: {
      backgroundColor: '#fff',
      marginVertical: 10,
      padding: 10,
      borderTopWidth: 1,
      borderTopColor: '#ccc',
    },
    sectionTitle: {
      fontSize: 20,
      fontWeight: 'bold',
      fontFamily: 'eb-garamond',
      marginBottom: 10,
    },
});