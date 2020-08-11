import Header from '../components/header'
import { AuthProps, privateRoute } from "../components/private_route";
import { Container, Col, Row } from 'react-bootstrap';
import Albums from '../components/albums';

function Home({auth}:AuthProps) {
  return <>
    <Header />
    <main className='main'>
      <Container>
        <Albums auth={auth} />
      </Container>
    </main>
  </>;
}

export default privateRoute(Home);