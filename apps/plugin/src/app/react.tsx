import { App } from "@/App";
import { createRoot } from "react-dom/client";

const rootElement = document.getElementById("root")!;
if (!rootElement.innerHTML) {
  const root = createRoot(rootElement);
  root.render(
    <App
      appPrompt={{
        applicationName: "Xelence 7.0",
        regionName: "Development",
        fileName: "wfmLogin",
        fileType: "Form",
        fileContent:
          '<!DOCTYPE html>\n<html lang="en">\n<head>\n <meta charset="UTF-8">\n <meta name="viewport" content="width=device-width, initial-scale=1.0">\n <title>Login Page</title>\n <link rel="stylesheet" href="style.css">\n</head>\n<body>\n <div class="login-container">\n <form class="login-form" id="loginForm">\n <h2>Login</h2>\n <div class="input-group">\n <label for="username">Username</label>\n <input type="text" id="username" name="username" required>\n </div>\n <div class="input-group">\n <label for="password">Password</label>\n <input type="password" id="password" name="password" required>\n </div>\n <button type="submit">Log In</button>\n <p id="message"></p>\n </form>\n </div>\n <script src="script.js"></script>\n</body>\n</html>',
        childFiles: [],
        userId: 1,
      }}
    />
  );
}
