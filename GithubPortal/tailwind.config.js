module.exports = {
    purge: {
        enabled: true,
        content: [
            './**/*.html',
            './**/*.razor'
        ],
    },
    darkMode: false, // or 'media' or 'class'
    theme: {
        extend: {
            maxWidth: {
                '100px': '100px',
            },
            colors: {
                blue: {
                    050: "#F0F8FA",
                    100: "#DAF1F7",
                    150: "#BFE2EB",
                    200: "#9DD3E3",
                    250: "#7CC0D9",
                    300: "#5DAFD1",
                    350: "#439AC1",
                    400: "#2B86B2",
                    450: "#1474A4",
                    500: "#176498",
                    550: "#1B578E",
                    600: "#1D4C84",
                    650: "#1C417A",
                    700: "#1A3670",
                    750: "#1C2D66",
                    800: "#1C255C",
                    850: "#191D4F",
                    900: "#0A0C1E"
                },
                gray: {
                    050: "#F5F5F5",
                    100: "#E8E8E8",
                    150: "#D6D6D6",
                    200: "#C4C4C4",
                    250: "#B0B0B0",
                    300: "#9E9E9E",
                    350: "#8C8C8C",
                    400: "#787878",
                    450: "#696969",
                    500: "#595959",
                    550: "#4D4D4D",
                    600: "#454545",
                    650: "#383838",
                    700: "#2B2B2B",
                    750: "#212121",
                    800: "#171717",
                    850: "#0E0E0E",
                    900: "#050505"
                }
            }
        }
    },
    variants: {
        extend: {},
    },
    plugins: [require('@tailwindcss/line-clamp')],
}