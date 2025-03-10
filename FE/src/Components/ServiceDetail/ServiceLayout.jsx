import styles from "./ServiceLayout.module.css";

const ServiceLayout = ({ service, serviceType, therapists = [] }) => {
  return (
    <div className={styles.layout}>
      <div className={styles.imageContainer}>
        <img
          src={service.imgUrl}
          alt={service.ServiceName}
          className={styles.image}
        />
      </div>

      <div className={styles.infoContainer}>
        <h1 className={styles.title}>{service.ServiceName}</h1>
        <p className={styles.description}>{service.Description}</p>

        <div className={styles.priceType}>
          <span className={styles.price}>
            ${service.Price.toLocaleString()}
          </span>
          <span className={styles.serviceType}>
            <span className={styles.dot}></span> {serviceType}
          </span>
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
          <button className={styles.order}>BOOK NOW</button>
        </div>
      </div>
    </div>
  );
};

export default ServiceLayout;
