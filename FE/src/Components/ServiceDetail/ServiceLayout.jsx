import styles from "./ServiceLayout.module.css";

const ServiceLayout = ({ service, serviceType, therapists = [], onBookButtonClick }) => {
  return (
    <div className={styles.layout}>
      <div className={styles.imageContainer}>
        <img
          src={service.imageUrl}
          alt={service.serviceName}
          className={styles.image}
        />
      </div>

      <div className={styles.infoContainer}>
        <h1 className={styles.title}>{service.serviceName}</h1>
        <p className={styles.description}>{service.description}</p>

        <div className={styles.detailsSection}>
          <span className={styles.price}>${service.price.toLocaleString()}</span>
          {service.serviceType && (
            <span className={styles.serviceType}>
              <span className={styles.dot}></span> {service.serviceType}
            </span>
          )}
        </div>

        <div className={styles.purchaseSection}>
          <select className={styles.specialistSelect}>
            <option value="">Let staff assign your therapist</option>
            {therapists.map((therapist) => (
              <option key={therapist.id} value={therapist.name}>
                {therapist.name}
              </option>
            ))}
          </select>
          <button className={styles.order} onClick={onBookButtonClick}>BOOK NOW</button>
        </div>
      </div>
    </div>
  );
};

export default ServiceLayout;
