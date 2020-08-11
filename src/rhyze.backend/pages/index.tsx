import Header from '../components/header'
import { AuthProps, privateRoute } from "../components/private_route";
import { Container, Col, Row } from 'react-bootstrap';

type Props = AuthProps;

function Home({auth}:Props) {
  return <>
    <Header />
    <main className='main'>
      <Container>
        <Row>
          <Col><h1 className="text-center">Welcome, {auth.decodedToken.email}</h1></Col>
        </Row>
      </Container>
    </main>
  </>;
}

export default privateRoute(Home);