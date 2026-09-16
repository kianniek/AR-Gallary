# AR-Gallery: VPS Kunstinstallatie

**AR-Gallery** is an Augmented Reality (AR) platform designed to integrate non-destructive, interactive digital art installations directly into public urban spaces. By utilizing a **Visual Positioning System (VPS)**, artists can place digital artwork at millimeter-level accuracy across cities without altering physical infrastructure or incurring physical maintenance costs.

The platform creates dynamic, location-based art routes that naturally blend into surrounding environments, allowing multiple virtual artworks to co-exist in the exact same physical space based on individual user preferences.

![ArtTech Poster](readme_attachments/ExpoCMDPosters2_Page_2.jpg)

---

## Project Overview

* **Project Name:** AR-Gallery
* **Sub-project:** VPS Kunstinstallatie (Stadsbrede AR Kunstinstallatie)
* **Domain:** Entertainment / Extended Reality (XR) / Cultural Tech
* **Goal:** Democratizing public space for digital art and bringing interactive digital experiences into the real world.

---

## Key Features

### For Visitors & Consumers
* **Location Notifications:** Receive automatic alerts when walking near AR installations in your area.
* **Personalized View:** Multiple artworks can inhabit the same location—users see artwork customized to their interests or preferences.
* **VPS Precision Positioning:** High-precision Visual Positioning System (VPS) ensures artwork stays anchored perfectly relative to surrounding architecture.
* **Interactive Art Routes:** Curated walking routes that bring cities to life through digital storytelling and spatial interaction.
* **Interactive Controls:** Tap, drag, scale, or trigger unique interactions built into 3D models.

### For Artists & Creators
* **Simple Upload Workflow:** Support for 3D model formats (.fbx, .gltf) with custom metadata (title, artist name, description, tags).
* **On-Site VPS Placement:** Intuitive tool for positioning, rotating, and scaling 3D models precisely on location using smartphone AR.
* **Interaction Setup:** Define interactive triggers (e.g., tap actions, animated responses, environmental cues).
* **Analytics Dashboard:** Track view metrics, total artwork interactions, and audience engagement.

---

## Gallery & Examples

| Duct Tape Banana Installation | Giant Clown Installation |
| :---: | :---: |
| ![Banana Thumbnail](readme_attachments/BananaTumbnail.jpg) | ![Clown Thumbnail](readme_attachments/Clowntumbnail.jpg) |

---

## User Flows

### 1. Consumer Discovery Flow

#### Triggered by Location Notification
Receive a notification when near an artwork, scan the environment, view the AR artwork, and open the detail sheet.

![Notification Flow](readme_attachments/Notificatie.jpg)

#### Spontaneous Discovery
Open the app, browse nearby artworks, locate the VPS anchor, and view/interact with the art.

![Spontaneous Flow](readme_attachments/spontaan%20openen.png)

---

### 2. Artist Creation & Placement Flow

Upload 3D assets (.fbx, .gltf), fill in artwork metadata, position the model on-site using AR controls, set up interactive elements, and publish.

![Artist Flow](readme_attachments/voor%20kunstenaars.jpg)

---

## Built With

* **Engine / Framework:** Unity (AR Foundation) / ARCore / ARKit
* **Localization & Anchoring:** Google Geospatial API / Niantic Lightship VPS / ARCore VPS
* **Asset Formats:** .fbx, .gltf
* **Target Platforms:** iOS & Android Smartphones

---

## Getting Started

### Prerequisites
* Unity 2022.3 LTS or higher
* Android device with ARCore support / iOS device with ARKit support
* Valid API key for VPS / Geospatial Location Services

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/kianniek/AR-Gallery.git
   cd AR-Gallery
   ```
2. Open the project in Unity Editor.
3. Ensure **Asset Serialization** is set to **Force Text** (*Edit > Project Settings > Editor*).
4. Configure your VPS/Geospatial API keys in `Project Settings > XR Plug-in Management`.
5. Build and deploy to your target mobile device.

---

## License
Distributed under the MIT License. See `LICENSE` for more information.
