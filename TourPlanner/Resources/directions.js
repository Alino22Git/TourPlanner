const directions = {
    "type": "FeatureCollection",
    "bbox": [
        16.377036,
        48.238922,
        16.378421,
        48.240183
    ],
    "features": [
        {
            "bbox": [
                16.377036,
                48.238922,
                16.378421,
                48.240183
            ],
            "type": "Feature",
            "properties": {
                "segments": [
                    {
                        "distance": 278.1,
                        "duration": 44.7,
                        "steps": [
                            {
                                "distance": 51.5,
                                "duration": 4.6,
                                "type": 11,
                                "instruction": "Head northwest on Dresdner Straße",
                                "name": "Dresdner Straße",
                                "way_points": [
                                    0,
                                    2
                                ]
                            },
                            {
                                "distance": 104.1,
                                "duration": 10.7,
                                "type": 1,
                                "instruction": "Turn right onto Höchstädtplatz",
                                "name": "Höchstädtplatz",
                                "way_points": [
                                    2,
                                    3
                                ]
                            },
                            {
                                "distance": 122.4,
                                "duration": 29.4,
                                "type": 3,
                                "instruction": "Turn sharp right onto Meldemannstraße",
                                "name": "Meldemannstraße",
                                "way_points": [
                                    3,
                                    5
                                ]
                            },
                            {
                                "distance": 0.0,
                                "duration": 0.0,
                                "type": 10,
                                "instruction": "Arrive at Meldemannstraße, on the right",
                                "name": "-",
                                "way_points": [
                                    5,
                                    5
                                ]
                            }
                        ]
                    }
                ],
                "summary": {
                    "distance": 278.1,
                    "duration": 44.7
                },
                "way_points": [
                    0,
                    5
                ]
            },
            "geometry": {
                "coordinates": [
                    [
                        16.377443,
                        48.238922
                    ],
                    [
                        16.377358,
                        48.239024
                    ],
                    [
                        16.377036,
                        48.239297
                    ],
                    [
                        16.377488,
                        48.240183
                    ],
                    [
                        16.378008,
                        48.23964
                    ],
                    [
                        16.378421,
                        48.239276
                    ]
                ],
                "type": "LineString"
            }
        }
    ],
    "metadata": {
        "attribution": "openrouteservice.org | OpenStreetMap contributors",
        "service": "routing",
        "timestamp": 1716042495947,
        "query": {
            "coordinates": [
                [
                    16.377631,
                    48.238992
                ],
                [
                    16.378317,
                    48.239223
                ]
            ],
            "profile": "driving-car",
            "format": "json"
        },
        "engine": {
            "version": "8.0.1",
            "build_date": "2024-05-14T10:47:52Z",
            "graph_date": "2024-05-14T22:04:28Z"
        }
    }
};
