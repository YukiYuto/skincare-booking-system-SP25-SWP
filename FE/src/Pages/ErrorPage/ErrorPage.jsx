import { Button, Typography, useTheme } from "@mui/material";
import styles from "./ErrorPage.module.css"; // Import file CSS module

const ErrorPage = () => {
  const theme = useTheme();

  return (
    <div className={styles.errorContainer}>
    <div className={styles.text}>
      <Typography
        variant="h1"
        sx={{
          fontSize: { xs: "4rem", md: "6rem" },
          fontWeight: 700,
          color: theme.palette.primary.main,
          marginBottom: 2,
          fontFamily: "'Playfair Display', serif"
        }}
      >
        404
      </Typography>

      <Typography
        variant="h4"
        sx={{
          marginBottom: 3,
          color: theme.palette.text.primary,
          fontFamily: "'Lato', sans-serif"
        }}
      >
        Oops! Page Not Found
      </Typography>

      <Typography
        variant="body1"
        sx={{
          marginBottom: 4,
          color: theme.palette.text.secondary,
          maxWidth: "600px"
        }}
      >
        We couldn not find the page you are looking for. It seems this page has
        vanished like your favorite moisturizer on a dry day.
      </Typography>

        <Button
          variant="contained"
          color="primary"
          className={styles.styledButton}
          href="/"
        >
          Return to Home
        </Button>
      
      </div>
    </div>
  );
};

export default ErrorPage;
