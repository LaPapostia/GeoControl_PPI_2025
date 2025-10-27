googleMapsInterop = {
    map: null,
    drawingManager: null,
    polygons: [],
    currentMarker: null,
    currentLatitude: 0,
    currentLongitude: 0,

    initMap: function (elementId, defaultLat, defaultLng, zoom, polygons_param = []) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (pos) => {
                    const lat = pos.coords.latitude;
                    const lng = pos.coords.longitude;

                    currentLatitude = lat;
                    currentLongitude = lng;

                    this.initializeMap(elementId, lat, lng, zoom, polygons_param);
                },
                (error) => {
                    console.warn('⚠️ No se pudo obtener la ubicación. Usando posición por defecto.', error);
                    this.initializeMap(elementId, defaultLat, defaultLng, zoom, polygons_param);
                },
                { enableHighAccuracy: true }
            );
        } else {
            console.warn('❌ Geolocalización no soportada. Usando posición por defecto.');
            this.initializeMap(elementId, defaultLat, defaultLng, zoom, polygons_param);
        }
    },

    initializeMap: function (elementId, lat, lng, zoom, polygons_param = []) {
        const mapOptions = {
            center: { lat: lat, lng: lng },
            zoom: zoom
        };

        this.map = new google.maps.Map(document.getElementById(elementId), mapOptions);

        // 🔹 Marcador de ubicación actual
        this.currentMarker = new google.maps.Marker({
            position: { lat: lat, lng: lng },
            map: this.map,
            title: "Tu ubicación actual"
        });

        // 🔹 Inicializar Drawing Manager
        this.drawingManager = new google.maps.drawing.DrawingManager({
            drawingMode: google.maps.drawing.OverlayType.POLYGON,
            drawingControl: false,
            drawingControlOptions: {
                position: google.maps.ControlPosition.TOP_CENTER,
                drawingModes: ['polygon'],
                editable: false
            },
            polygonOptions: {
                editable: false,
                draggable: false
            }
        });

        this.drawingManager.setMap(this.map);

        // 🔹 Evento al crear un nuevo polígono manualmente
        google.maps.event.addListener(this.drawingManager, 'overlaycomplete', (event) => {
            if (event.type === 'polygon') {
                const path = event.overlay.getPath().getArray().map(p => ({
                    lat: p.lat(),
                    lng: p.lng()
                }));
                this.polygons.push(event.overlay);
                DotNet.invokeMethodAsync('Geocontrol_PPI_NET_9.Web', 'OnPolygonCreated', JSON.stringify(path));
            }
        });

        // 🔹 Cargar polígonos iniciales desde parámetro
        if (Array.isArray(polygons_param) && polygons_param.length > 0) {
            polygons_param.forEach(points => {
                this.addPolygon(points, false, false);
            });
        }
    },

    addPolygon: function (points, editable, draggable) {
        const polygon = new google.maps.Polygon({
            paths: points,
            map: this.map,
            strokeColor: "#FF0000",
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: "#FF0000",
            fillOpacity: 0.35,
            editable: editable,
            draggable: draggable
        });
        this.polygons.push(polygon);
    },

    setPolygonsEditable: function (editable, draggable = true) {
        if (!this.polygons || this.polygons.length === 0) return;

        this.polygons.forEach(polygon => {
            polygon.setEditable(editable);
            polygon.setDraggable(editable && draggable);
        });

        this.drawingManager.setOptions({
            drawingMode: editable ? google.maps.drawing.OverlayType.POLYGON : null,
            drawingControl: editable
        });
    },

    isPositionInsidePolygons: function () {

        if (!this.polygons || this.polygons.length === 0) {
            console.warn("⚠️ No hay polígonos cargados para validar.");
            return false;
        }

        const lat = currentLatitude;
        const lng = currentLongitude;

        const point = new google.maps.LatLng(lat, lng);
        for (const polygon of this.polygons) {
            if (google.maps.geometry.poly.containsLocation(point, polygon)) {
                console.log("✅ La posición está dentro de un polígono existente.");
                return true;
            }
        }

        console.log("❌ La posición está fuera de todos los polígonos.");
        return false;
    }
};

async function requestGeolocationPermission() {
    try {
        const status = await navigator.permissions.query({ name: 'geolocation' });

        if (status.state === 'granted') {
            return true;
        } else if (status.state === 'prompt') {
            return true;
        } else {
            return false;
        }
    } catch (error) {
        console.error('Error verificando permisos:', error);
    }
}

async function startTrackingLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.watchPosition(
            (pos) => {
                const lat = pos.coords.latitude;
                const lng = pos.coords.longitude;
                DotNet.invokeMethodAsync('BlazorAppDemo', 'UpdateMapPosition', lat, lng);
            },
            (error) => console.error('Error en seguimiento:', error),
            { enableHighAccuracy: true }
        );
    }
};
